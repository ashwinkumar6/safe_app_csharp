﻿using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using SafeApp.Utilities;

namespace SafeApp.MockAuthBindings
{
    /// <summary>
    /// Represents an application registered with the authenticator.
    /// </summary>
    [PublicAPI]
    public struct RegisteredApp
    {
        /// <summary>
        /// Application exchange info.
        /// </summary>
        public AppExchangeInfo AppInfo;

        /// <summary>
        /// List of containers the application has access to.
        /// </summary>
        public List<ContainerPermissions> Containers;

        /// <summary>
        /// Initialize new registered app object using native registered app.
        /// </summary>
        /// <param name="native"></param>
        internal RegisteredApp(RegisteredAppNative native)
        {
            AppInfo = native.AppInfo;
            Containers = BindingUtils.CopyToObjectList<ContainerPermissions>(native.ContainersPtr, (int)native.ContainersLen);
        }

        /// <summary>
        /// Returns native registered app
        /// </summary>
        /// <returns></returns>
        internal RegisteredAppNative ToNative()
        {
            return new RegisteredAppNative
            {
                AppInfo = AppInfo,
                ContainersPtr = BindingUtils.CopyFromObjectList(Containers),
                ContainersLen = (UIntPtr)(Containers?.Count ?? 0),
                ContainersCap = UIntPtr.Zero
            };
        }
    }

    /// <summary>
    /// Represents a native application registered with the authenticator.
    /// </summary>
    internal struct RegisteredAppNative
    {
        /// <summary>
        /// Application exchange info.
        /// </summary>
        public AppExchangeInfo AppInfo;

        /// <summary>
        /// Pointer to the array of ContainerInfo.
        /// </summary>
        public IntPtr ContainersPtr;

        /// <summary>
        /// Length of containers array.
        /// </summary>
        public UIntPtr ContainersLen;

        /// <summary>
        /// Capacity of the containers array. Internal data required for the Rust allocator.
        /// </summary>
        // ReSharper disable once NotAccessedField.Compiler
        public UIntPtr ContainersCap;

        /// <summary>
        /// Used to free the pointers to array.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        internal void Free()
        {
            BindingUtils.FreeList(ref ContainersPtr, ref ContainersLen);
        }
    }
}
