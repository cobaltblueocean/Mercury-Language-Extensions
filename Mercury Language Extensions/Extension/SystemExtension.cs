// Copyright (c) 2017 - presented by Kei Nakai
//
// Please see distribution for license.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
//     
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace System
{
    public static class SystemExtension
    {
        public static T[] CloneExact<T>(this T[] originalArray)
        {
            return (T[])originalArray.Clone();
        }

        public static T CloneExact<T>(this T source)
        {
            try
            {
                var serialized = System.Text.Json.JsonSerializer.Serialize<T>(source);
                T retVal = System.Text.Json.JsonSerializer.Deserialize<T>(serialized);

                return retVal;
            }
            catch (System.NotSupportedException ne)
            {
                try
                {
                    T ret = Core.CreateInstanceFromType(source.GetType());
                    Core.CopyProperties(source, ret);

                    return ret;
                }
                catch
                {
                    throw;
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
