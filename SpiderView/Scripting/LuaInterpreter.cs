﻿using LuaInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spider.Scripting
{
   
    /// <summary>
    /// Lua interpreter for the Spider view
    /// </summary>
    public class LuaInterpreter : Interpreter
    {
        private LuaInterface.Lua lua;
        private SpiderView host;
        
        public LuaInterpreter(SpiderView host)
        {
            this.host = host;
            this.lua = new LuaInterface.Lua();
            this.lua.DoString(Properties.Resources.helperlua); // Add helper methods
        }
        public string ContentType
        {
            get { return "text/lua"; }
        }

        public void LoadFile(string fileName)
        {
            this.lua.LoadFile(fileName);
            this.lua.DoString(Properties.Resources.helperlua); // Add helper methods
        }
        public void LoadScript(string code)
        {
          
            this.lua.DoString(code, "");
        }
        public object[] RunCode(string code)
        {
            return this.lua.DoString(code);
        }

        public void RegisterFunction(string function, System.Reflection.MethodBase func, Object target)
        {
            this.lua.RegisterFunction(function, target, func);
        }

        public SpiderView View
        {
            get { return this.host; }
        }

        /// <summary>
        /// Invoke the function
        /// </summary>
        /// <param name="function"></param>
        /// <param name="arguments"></param>
        public object[] InvokeFunction(string function, params object[] arguments)
        {
            try
            {
                
                LuaFunction func = this.lua.GetFunction( function);
                return func.Call(arguments);
            }
            catch (Exception e)
            {
                return new Object[] { };
            }
        }
        private LuaTable ToLuaTable(MemoryTable table)
        {
            String name = "memory_table_" + new Random().Next().ToString();
            lua.NewTable(name);
            LuaTable newTable = lua.GetTable(name);
            foreach (KeyValuePair<String, Object> obj in newTable)
            {
                if (obj.GetType() == typeof(MemoryTable))
                {
                    newTable[obj.Key] = ToLuaTable((MemoryTable)obj.Value);
                }
                else
                {
                    newTable[obj.Key] = obj.Value;
                }
            }
            return newTable;

        
        }
        /// <summary>
        /// Invoke the function
        /// </summary>
        /// <param name="function"></param>
        /// <param name="arguments"></param>
        public object[] InvokeFunction(string function, MemoryTable table)
        {
            try
            {
                

                LuaFunction func = this.lua.GetFunction(function);
                return func.Call(new Object[]{ToLuaTable(table)});
            }
            catch (Exception e)
            {
                return new Object[] { };
            }
        }

        public void RegisterFunction(string function, Delegate func, Object target)
        {
           

             this.lua.RegisterFunction(function,target, func.Method);
            
        }


        public void SetVariable(string variable, object val)
        {
            
            this.lua[variable] = val;
        }


        public object TableToNative(MemoryTable table)
        {
            return ToLuaTable(table);
        }
    }
}
