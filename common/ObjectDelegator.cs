using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace Revival.Common
{
    public class ObjectDelegator : IEnumerable
    {
        public delegate object FieldGetValueDelegate(object target);

        public delegate void FieldSetValueDelegate(object target, object value);

        private readonly Dictionary<string, FieldDelegate>
            _fieldDelegatesDict = new Dictionary<string, FieldDelegate>();

        public readonly List<FieldDelegate> FieldDelegatesList = new List<FieldDelegate>();
        public readonly List<FieldDelegate> FieldDelegatesPublicList = new List<FieldDelegate>();

        ///// <summary>
        ///// Create field delegators for every field in a type.
        ///// The delegators will be created for all Public, NonPublic, and Instance fields.
        ///// </summary>
        ///// <param name="type">The type to create the deletes from.</param>
        //public ObjectDelegator(Type type)
        //{
        //    if (type == null) throw new ArgumentNullException(nameof(type), "Cannot be null!");

        //    FieldInfo[] fieldInfos =
        //        type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        //    foreach (FieldInfo fieldInfo in fieldInfos)
        //    {
        //        AddField(fieldInfo);
        //    }
        //}

        /// <summary>
        ///     Create field delegators from an array of field infos.
        ///     The supplied array should be ordered as desired for public field ordering to work.
        /// </summary>
        /// <param name="fieldInfos">The fields to create delegates from.</param>
        public ObjectDelegator(IEnumerable<FieldInfo> fieldInfos)
        {
            if (fieldInfos == null)
            {
                throw new ArgumentNullException(nameof(fieldInfos), "Cannot be null!");
            }

            foreach (var fieldInfo in fieldInfos)
            {
                AddField(fieldInfo);
            }
        }

        public int FieldCount => _fieldDelegatesDict.Count;
        public int PublicFieldCount => FieldDelegatesPublicList.Count;

        // "getter"
        public FieldGetValueDelegate this[string fieldName] => GetFieldGetDelegate(fieldName);

        // "setter"
        public object this[string fieldName, object target]
        {
            set
            {
                var fieldSetDelegate = GetFieldSetDelegate(fieldName);
                fieldSetDelegate?.Invoke(target, value);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void AddField(FieldInfo fieldInfo)
        {
            if (_fieldDelegatesDict.ContainsKey(fieldInfo.Name))
            {
                return;
            }

            var fieldDelegate = new FieldDelegate
            {
                Name = fieldInfo.Name,
                GetValue = _CreateGetField(fieldInfo),
                SetValue = _CreateSetField(fieldInfo),
                Info = fieldInfo
            };

            _fieldDelegatesDict.Add(fieldInfo.Name, fieldDelegate);

            FieldDelegatesList.Add(fieldDelegate);
            if (fieldInfo.IsPublic)
            {
                FieldDelegatesPublicList.Add(fieldDelegate);
            }
        }

        private static FieldSetValueDelegate _CreateSetField(FieldInfo field)
        {
            // This cannot be null
            if (field.DeclaringType == null)
            {
                throw new ArgumentNullException(nameof(field), "Cannot have a null owner value.");
            }

            // create our custom delegate method
            var setMethod = new DynamicMethod("SetValue", typeof(void), new[] {typeof(object), typeof(object)},
                field.DeclaringType);
            var ilGenerator = setMethod.GetILGenerator();

            // push the object to read, cast to our type, then push the value to set to
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Castclass, field.DeclaringType);
            ilGenerator.Emit(OpCodes.Ldarg_1);

            // if the field is a primitive then we need to unbox it
            if (field.FieldType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Unbox_Any, field.FieldType);
            }

            // set the field
            ilGenerator.Emit(OpCodes.Stfld, field);

            // return the value
            ilGenerator.Emit(OpCodes.Ret);

            return (FieldSetValueDelegate) setMethod.CreateDelegate(typeof(FieldSetValueDelegate));
        }

        private static FieldGetValueDelegate _CreateGetField(FieldInfo field)
        {
            // This cannot be null.
            if (field.DeclaringType == null)
            {
                throw new ArgumentNullException(nameof(field), "Cannot have a null owner value.");
            }

            // create our custom delegate method
            var getMethod =
                new DynamicMethod("GetValue", typeof(object), new[] {typeof(object)}, field.DeclaringType);
            var ilGenerator = getMethod.GetILGenerator();

            // push the object to read, cast to our type, and get the field
            ilGenerator.Emit(OpCodes.Ldarg_0);
            ilGenerator.Emit(OpCodes.Castclass, field.DeclaringType);
            ilGenerator.Emit(OpCodes.Ldfld, field);

            // if the field is a primitive then we need to box it
            if (field.FieldType.IsValueType)
            {
                ilGenerator.Emit(OpCodes.Box, field.FieldType);
            }

            // return the value
            ilGenerator.Emit(OpCodes.Ret);

            return (FieldGetValueDelegate) getMethod.CreateDelegate(typeof(FieldGetValueDelegate));
        }

        public FieldDelegate GetFieldDelegate(string fieldName)
        {
            return _fieldDelegatesDict.TryGetValue(fieldName, out var fieldDelegate) ? fieldDelegate : null;
        }

        public FieldDelegate GetPublicFieldDelegate(int index)
        {
            if (index < 0 || index > FieldDelegatesPublicList.Count)
            {
                return null;
            }

            return FieldDelegatesPublicList[index];
        }

        public FieldGetValueDelegate GetFieldGetDelegate(string fieldName)
        {
            return _fieldDelegatesDict.TryGetValue(fieldName, out var fieldDelegate) ? fieldDelegate.GetValue : null;
        }

        public FieldSetValueDelegate GetFieldSetDelegate(string fieldName)
        {
            return _fieldDelegatesDict.TryGetValue(fieldName, out var fieldDelegate) ? fieldDelegate.SetValue : null;
        }

        public bool ContainsGetFieldDelegate(string fieldName)
        {
            return _fieldDelegatesDict.ContainsKey(fieldName);
        }

        public DelegatorEnumerator GetEnumerator()
        {
            return new DelegatorEnumerator(_fieldDelegatesDict);
        }

        public class FieldDelegate
        {
            public FieldGetValueDelegate GetValue;
            public FieldInfo Info;
            public string Name;
            public FieldSetValueDelegate SetValue;

            public Type FieldType => Info.FieldType;
            public bool IsPublic => Info.IsPublic;
            public bool IsPrivate => Info.IsPrivate;
        }

        public class DelegatorEnumerator : IEnumerator
        {
            private readonly Dictionary<string, FieldDelegate> _delegatesDict;
            private Dictionary<string, FieldDelegate>.Enumerator _delegatesEnumerator;

            public DelegatorEnumerator(Dictionary<string, FieldDelegate> delegatesDict)
            {
                _delegatesDict = delegatesDict;
                Reset();
            }

            public FieldDelegate Current
            {
                get
                {
                    try
                    {
                        return _delegatesEnumerator.Current.Value;
                    }
                    catch (IndexOutOfRangeException)
                    {
                        throw new InvalidOperationException();
                    }
                }
            }

            public bool MoveNext()
            {
                return _delegatesEnumerator.MoveNext();
            }

            public void Reset()
            {
                _delegatesEnumerator = _delegatesDict.GetEnumerator();
            }

            object IEnumerator.Current => Current;
        }
    }
}