﻿// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version: 17.0.0.0
//  
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------
namespace ReactiveMemory.GeneratorCore
{
    using System.Linq;
    using System.Text;
    using System.Collections.Generic;
    using System;
    
    /// <summary>
    /// Class to produce the template output
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public partial class DataBaseContextTemplate : DataBaseContextTemplateBase
    {
        /// <summary>
        /// Create the template output
        /// </summary>
        public virtual string TransformText()
        {
            this.Write("// <auto-generated />\r\n#pragma warning disable CS0105\r\n");
            this.Write(this.ToStringHelper.ToStringWithCulture(Using));
            this.Write("\r\n\r\nnamespace ");
            this.Write(this.ToStringHelper.ToStringWithCulture(Namespace));
            this.Write("\r\n{\r\n   public sealed class ");
            this.Write(this.ToStringHelper.ToStringWithCulture(ClassName));
            this.Write("  : IDisposable\r\n   {\r\n        public bool IsTransactionStarted { get; private se" +
                    "t; }\r\n        public event Action OnUnauthorizedMemoryModification;\r\n\t\tpublic I");
            this.Write(this.ToStringHelper.ToStringWithCulture(MemoryDatabaseClassName));
            this.Write("  Database => _database ??= new ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MemoryDatabaseClassName));
            this.Write("(_data, _changesConveyor, maxDegreeOfParallelism : Environment.ProcessorCount);\r\n" +
                    "        public I");
            this.Write(this.ToStringHelper.ToStringWithCulture(TransactionClassName));
            this.Write("  Transaction => _transaction;\r\n        public event Action OnTransactionFinished" +
                    ";\r\n\r\n        private ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MemoryDatabaseClassName));
            this.Write(" _database;\r\n        private ");
            this.Write(this.ToStringHelper.ToStringWithCulture(TransactionClassName));
            this.Write(@" _transaction;
        private ChangesConveyor _changesConveyor;
        private byte[] _data;
        private byte[] _hash;
        private MD5 _md5;

        public DbContext(byte[] dbBytes, IChangesMediatorFactory changesMediatorFactory, string md5 = """")
        {
            _changesConveyor = new ChangesConveyor(changesMediatorFactory);
            _data = dbBytes;
            if(!string.IsNullOrWhiteSpace(md5))
			{
				_md5 = MD5.Create(md5);
				_hash = _md5.ComputeHash(_data);
			}
        }

        public I");
            this.Write(this.ToStringHelper.ToStringWithCulture(TransactionClassName));
            this.Write(@" BeginTransaction()
        {
            if (IsTransactionStarted)
            {
                throw new InvalidOperationException(""Transaction is already started"");
            }

            IsTransactionStarted = true;
            if (_database == null)
            {
                // serialization of db from bytes, it's slow
                _database = new ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MemoryDatabaseClassName));
            this.Write(@"(_data, _changesConveyor, maxDegreeOfParallelism : Environment.ProcessorCount);
            }
            else if(_md5 != null)
            {
                // calc hash of current db data
                var prevDataHash = _md5.ComputeHash(_data); 
                if (!prevDataHash.SequenceEqual(_hash) || !prevDataHash.SequenceEqual(_md5.ComputeHash(ToBytes())))
                {
                    // detected memory modifications
                    OnUnauthorizedMemoryModification?.Invoke();
                }
            }
            // it just cast, but when we make changes it make copy of data, so Database will not be changed
            _transaction = _database.BeginTransaction();
            return _transaction;
        }


        public void Commit()
        {
            if (!IsTransactionStarted)
            {
                throw new InvalidOperationException(""Transaction is not started"");
            }

            // cast to  ");
            this.Write(this.ToStringHelper.ToStringWithCulture(MemoryDatabaseClassName));
            this.Write(@" 
            _database = _transaction.Commit();
            OnTransactionFinished?.Invoke();
            if(_md5 != null)
			{
				_data = ToBytes();
				_hash = _md5.ComputeHash(_data);
			}
            IsTransactionStarted = false;
        }

        public void Rollback()
        {
            // all changes in Transaction, so we just set it to null to discard changes
            _database.ChangesConveyor.Clear();
            _transaction = null;
            IsTransactionStarted = false;
        }
        
        public byte[] ToBytes()
        {
            return _database.ToDatabaseBuilder().Build();
        }

        public void Dispose()
        {
            _changesConveyor?.Dispose();
            _md5?.Dispose();
        }
   }
}");
            return this.GenerationEnvironment.ToString();
        }
    }
    #region Base class
    /// <summary>
    /// Base class for this transformation
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.TextTemplating", "17.0.0.0")]
    public class DataBaseContextTemplateBase
    {
        #region Fields
        private global::System.Text.StringBuilder generationEnvironmentField;
        private global::System.CodeDom.Compiler.CompilerErrorCollection errorsField;
        private global::System.Collections.Generic.List<int> indentLengthsField;
        private string currentIndentField = "";
        private bool endsWithNewline;
        private global::System.Collections.Generic.IDictionary<string, object> sessionField;
        #endregion
        #region Properties
        /// <summary>
        /// The string builder that generation-time code is using to assemble generated output
        /// </summary>
        public System.Text.StringBuilder GenerationEnvironment
        {
            get
            {
                if ((this.generationEnvironmentField == null))
                {
                    this.generationEnvironmentField = new global::System.Text.StringBuilder();
                }
                return this.generationEnvironmentField;
            }
            set
            {
                this.generationEnvironmentField = value;
            }
        }
        /// <summary>
        /// The error collection for the generation process
        /// </summary>
        public System.CodeDom.Compiler.CompilerErrorCollection Errors
        {
            get
            {
                if ((this.errorsField == null))
                {
                    this.errorsField = new global::System.CodeDom.Compiler.CompilerErrorCollection();
                }
                return this.errorsField;
            }
        }
        /// <summary>
        /// A list of the lengths of each indent that was added with PushIndent
        /// </summary>
        private System.Collections.Generic.List<int> indentLengths
        {
            get
            {
                if ((this.indentLengthsField == null))
                {
                    this.indentLengthsField = new global::System.Collections.Generic.List<int>();
                }
                return this.indentLengthsField;
            }
        }
        /// <summary>
        /// Gets the current indent we use when adding lines to the output
        /// </summary>
        public string CurrentIndent
        {
            get
            {
                return this.currentIndentField;
            }
        }
        /// <summary>
        /// Current transformation session
        /// </summary>
        public virtual global::System.Collections.Generic.IDictionary<string, object> Session
        {
            get
            {
                return this.sessionField;
            }
            set
            {
                this.sessionField = value;
            }
        }
        #endregion
        #region Transform-time helpers
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void Write(string textToAppend)
        {
            if (string.IsNullOrEmpty(textToAppend))
            {
                return;
            }
            // If we're starting off, or if the previous text ended with a newline,
            // we have to append the current indent first.
            if (((this.GenerationEnvironment.Length == 0) 
                        || this.endsWithNewline))
            {
                this.GenerationEnvironment.Append(this.currentIndentField);
                this.endsWithNewline = false;
            }
            // Check if the current text ends with a newline
            if (textToAppend.EndsWith(global::System.Environment.NewLine, global::System.StringComparison.CurrentCulture))
            {
                this.endsWithNewline = true;
            }
            // This is an optimization. If the current indent is "", then we don't have to do any
            // of the more complex stuff further down.
            if ((this.currentIndentField.Length == 0))
            {
                this.GenerationEnvironment.Append(textToAppend);
                return;
            }
            // Everywhere there is a newline in the text, add an indent after it
            textToAppend = textToAppend.Replace(global::System.Environment.NewLine, (global::System.Environment.NewLine + this.currentIndentField));
            // If the text ends with a newline, then we should strip off the indent added at the very end
            // because the appropriate indent will be added when the next time Write() is called
            if (this.endsWithNewline)
            {
                this.GenerationEnvironment.Append(textToAppend, 0, (textToAppend.Length - this.currentIndentField.Length));
            }
            else
            {
                this.GenerationEnvironment.Append(textToAppend);
            }
        }
        /// <summary>
        /// Write text directly into the generated output
        /// </summary>
        public void WriteLine(string textToAppend)
        {
            this.Write(textToAppend);
            this.GenerationEnvironment.AppendLine();
            this.endsWithNewline = true;
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void Write(string format, params object[] args)
        {
            this.Write(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Write formatted text directly into the generated output
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            this.WriteLine(string.Format(global::System.Globalization.CultureInfo.CurrentCulture, format, args));
        }
        /// <summary>
        /// Raise an error
        /// </summary>
        public void Error(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Raise a warning
        /// </summary>
        public void Warning(string message)
        {
            System.CodeDom.Compiler.CompilerError error = new global::System.CodeDom.Compiler.CompilerError();
            error.ErrorText = message;
            error.IsWarning = true;
            this.Errors.Add(error);
        }
        /// <summary>
        /// Increase the indent
        /// </summary>
        public void PushIndent(string indent)
        {
            if ((indent == null))
            {
                throw new global::System.ArgumentNullException("indent");
            }
            this.currentIndentField = (this.currentIndentField + indent);
            this.indentLengths.Add(indent.Length);
        }
        /// <summary>
        /// Remove the last indent that was added with PushIndent
        /// </summary>
        public string PopIndent()
        {
            string returnValue = "";
            if ((this.indentLengths.Count > 0))
            {
                int indentLength = this.indentLengths[(this.indentLengths.Count - 1)];
                this.indentLengths.RemoveAt((this.indentLengths.Count - 1));
                if ((indentLength > 0))
                {
                    returnValue = this.currentIndentField.Substring((this.currentIndentField.Length - indentLength));
                    this.currentIndentField = this.currentIndentField.Remove((this.currentIndentField.Length - indentLength));
                }
            }
            return returnValue;
        }
        /// <summary>
        /// Remove any indentation
        /// </summary>
        public void ClearIndent()
        {
            this.indentLengths.Clear();
            this.currentIndentField = "";
        }
        #endregion
        #region ToString Helpers
        /// <summary>
        /// Utility class to produce culture-oriented representation of an object as a string.
        /// </summary>
        public class ToStringInstanceHelper
        {
            private System.IFormatProvider formatProviderField  = global::System.Globalization.CultureInfo.InvariantCulture;
            /// <summary>
            /// Gets or sets format provider to be used by ToStringWithCulture method.
            /// </summary>
            public System.IFormatProvider FormatProvider
            {
                get
                {
                    return this.formatProviderField ;
                }
                set
                {
                    if ((value != null))
                    {
                        this.formatProviderField  = value;
                    }
                }
            }
            /// <summary>
            /// This is called from the compile/run appdomain to convert objects within an expression block to a string
            /// </summary>
            public string ToStringWithCulture(object objectToConvert)
            {
                if ((objectToConvert == null))
                {
                    throw new global::System.ArgumentNullException("objectToConvert");
                }
                System.Type t = objectToConvert.GetType();
                System.Reflection.MethodInfo method = t.GetMethod("ToString", new System.Type[] {
                            typeof(System.IFormatProvider)});
                if ((method == null))
                {
                    return objectToConvert.ToString();
                }
                else
                {
                    return ((string)(method.Invoke(objectToConvert, new object[] {
                                this.formatProviderField })));
                }
            }
        }
        private ToStringInstanceHelper toStringHelperField = new ToStringInstanceHelper();
        /// <summary>
        /// Helper to produce culture-oriented representation of an object as a string
        /// </summary>
        public ToStringInstanceHelper ToStringHelper
        {
            get
            {
                return this.toStringHelperField;
            }
        }
        #endregion
    }
    #endregion
}
