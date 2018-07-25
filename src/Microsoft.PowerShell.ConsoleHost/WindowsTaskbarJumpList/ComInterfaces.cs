﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace Microsoft.PowerShell
{
    internal static class ComInterfaces
    {
        [DllImport("kernel32.dll", SetLastError = true, EntryPoint = "GetStartupInfoA")]
        internal static extern void GetStartupInfo(out StartUpInfo lpStartupInfo);

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        internal struct StartUpInfo
        {
            public Int32 cb;
            public string lpReserved;
            public string lpDesktop;
            public string lpTitle;
            public Int32 dwX;
            public Int32 dwY;
            public Int32 dwXSize;
            public Int32 dwYSize;
            public Int32 dwXCountChars;
            public Int32 dwYCountChars;
            public Int32 dwFillAttribute;
            public Int32 dwFlags;
            public Int16 wShowWindow;
            public Int16 cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        [ComImport]
        [Guid("00021401-0000-0000-C000-000000000046")]
        [ClassInterface(ClassInterfaceType.None)]
        internal class CShellLink { }

        [ComImport]
        [Guid("000214F9-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IShellLinkW
        {
            void GetPath(
                [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
                int cchMaxPath,
                IntPtr pfd,
                uint fFlags);
            void GetIDList(out IntPtr ppidl);
            void SetIDList(IntPtr pidl);
            void GetDescription(
                [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszFile,
                int cchMaxName);
            void SetDescription(
                [MarshalAs(UnmanagedType.LPWStr)] string pszName);
            void GetWorkingDirectory(
                [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszDir,
                int cchMaxPath
                );
            void SetWorkingDirectory(
                [MarshalAs(UnmanagedType.LPWStr)] string pszDir);
            void GetArguments(
                [Out(), MarshalAs(UnmanagedType.LPWStr)] StringBuilder pszArgs,
                int cchMaxPath);
            void SetArguments(
                [MarshalAs(UnmanagedType.LPWStr)] string pszArgs);
            void GetHotKey(out short wHotKey);
            void SetHotKey(short wHotKey);
            void GetShowCmd(out uint iShowCmd);
            void SetShowCmd(uint iShowCmd);
            void GetIconLocation(
                [Out(), MarshalAs(UnmanagedType.LPWStr)] out StringBuilder pszIconPath,
                int cchIconPath,
                out int iIcon);
            void SetIconLocation(
                [MarshalAs(UnmanagedType.LPWStr)] string pszIconPath,
                int iIcon);
            void SetRelativePath(
                [MarshalAs(UnmanagedType.LPWStr)] string pszPathRel,
                uint dwReserved);
            void Resolve(IntPtr hwnd, uint fFlags);
            void SetPath(
                [MarshalAs(UnmanagedType.LPWStr)] string pszFile);
        }

        /// <summary>
        /// A property store.
        /// </summary>
        [ComImport]
        [Guid("886D8EEB-8CF2-4446-8D02-CDBA1DBDCF99")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IPropertyStore
        {
            /// <summary>
            /// Gets the number of properties contained in the property store.
            /// </summary>
            /// <param name="propertyCount"></param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            HResult GetCount([Out] out uint propertyCount);

            /// <summary>
            /// Get a property key located at a specific index.
            /// </summary>
            /// <param name="propertyIndex"></param>
            /// <param name="key"></param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            HResult GetAt([In] uint propertyIndex, out PropertyKey key);

            /// <summary>
            /// Gets the value of a property from the store.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="pv"></param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            HResult GetValue([In] ref PropertyKey key, [Out] PropVariant pv);

            /// <summary>
            /// Sets the value of a property in the store.
            /// </summary>
            /// <param name="key"></param>
            /// <param name="pv"></param>
            /// <returns></returns>
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), PreserveSig]
            HResult SetValue([In] ref PropertyKey key, [In] PropVariant pv);

            /// <summary>
            /// Commits the changes.
            /// </summary>
            /// <returns></returns>
            [PreserveSig]
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            HResult Commit();
        }

        [ComImport()]
        [Guid("6332DEBF-87B5-4670-90C0-5E57B408A49E")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface ICustomDestinationList
        {
            void SetAppID(
                [MarshalAs(UnmanagedType.LPWStr)] string pszAppID);
            [PreserveSig]
            HResult BeginList(
                out uint cMaxSlots,
                ref Guid riid,
                [Out(), MarshalAs(UnmanagedType.Interface)] out object ppvObject);
            [PreserveSig]
            HResult AppendCategory(
                [MarshalAs(UnmanagedType.LPWStr)] string pszCategory,
                [MarshalAs(UnmanagedType.Interface)] IObjectArray poa);
            void AppendKnownCategory(
                [MarshalAs(UnmanagedType.I4)] KnownDestinationCategory category);
            [PreserveSig]
            HResult AddUserTasks(
                [MarshalAs(UnmanagedType.Interface)] IObjectArray poa);
            void CommitList();
            void GetRemovedDestinations(
                ref Guid riid,
                [Out(), MarshalAs(UnmanagedType.Interface)] out object ppvObject);
            void DeleteList(
                [MarshalAs(UnmanagedType.LPWStr)] string pszAppID);
            void AbortList();
        }

        internal enum KnownDestinationCategory
        {
            Frequent = 1,
            Recent
        }

        [ComImport()]
        [Guid("92CA9DCD-5622-4BBA-A805-5E9F541BD8C9")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IObjectArray
        {
            void GetCount(out uint cObjects);
            void GetAt(
                uint iIndex,
                ref Guid riid,
                [Out(), MarshalAs(UnmanagedType.Interface)] out object ppvObject);
        }

        [ComImport()]
        [Guid("5632B1A4-E38A-400A-928A-D4CD63230295")]
        [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IObjectCollection
        {
            // IObjectArray
            [PreserveSig]
            void GetCount(out uint cObjects);
            [PreserveSig]
            void GetAt(
                uint iIndex,
                ref Guid riid,
                [Out(), MarshalAs(UnmanagedType.Interface)] out object ppvObject);

            // IObjectCollection
            void AddObject(
                [MarshalAs(UnmanagedType.Interface)] object pvObject);
            void AddFromArray(
                [MarshalAs(UnmanagedType.Interface)] IObjectArray poaSource);
            void RemoveObject(uint uiIndex);
            void Clear();
        }

        [ComImport]
        [Guid("45e2b4ae-b1c3-11d0-b92f-00a0c90312e1"),
        InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        internal interface IShellLinkDataListW
        {
            [PreserveSig]
            Int32 AddDataBlock(IntPtr pDataBlock);

            [PreserveSig]
            Int32 CopyDataBlock(UInt32 dwSig, out IntPtr ppDataBlock);

            [PreserveSig]
            Int32 RemoveDataBlock(UInt32 dwSig);

            void GetFlags(out uint pdwFlags);
            void SetFlags(uint dwFlags);
        }

        [DllImport("ole32.Dll")]
        static internal extern HResult CoCreateInstance(ref Guid clsid,
           [MarshalAs(UnmanagedType.IUnknown)] object inner,
           uint context,
           ref Guid uuid,
           [MarshalAs(UnmanagedType.IUnknown)] out object rReturnedComObject);
    }
}