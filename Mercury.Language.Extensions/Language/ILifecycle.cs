// Copyright (c) 2017 - presented by Kei Nakai
//
// Original project is developed and published by OpenGamma Inc.
//
// Copyright (C) 2012 - present by OpenGamma Inc. and the OpenGamma group of companies
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

namespace System
{
    public interface ILifecycle
    {
        /// <summary>
        /// Start this component.
        /// Should not throw an exception if the component is already running.
        /// In the case of a container, this will propagate the start signal to all components that apply.
        /// </summary>
        void Start();

        /// <summary>
        /// Stop this component, typically in a synchronous fashion, such that the component is fully stopped upon return of this method. Consider implementing SmartLifecycle and its stop(Runnable) variant when asynchronous stop behavior is necessary.
        /// Note that this stop notification is not guaranteed to come before destruction: On regular shutdown, Lifecycle beans will first receive a stop notification before the general destruction callbacks are being propagated; however, on hot refresh during a context's lifetime or on aborted refresh attempts, only destroy methods will be called.
        /// 
        /// Should not throw an exception if the component isn't started yet.
        /// 
        /// In the case of a container, this will propagate the stop signal to all components that apply.
        /// </summary>
        void Stop();

        /// <summary>
        /// Check whether this component is currently running.
        /// In the case of a container, this will return true only if all components that apply are currently running.
        /// </summary>
        Boolean IsRunning { get; }
    }
}
