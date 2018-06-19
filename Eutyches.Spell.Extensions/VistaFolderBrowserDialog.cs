﻿//=============================================================================
// 
//=============================================================================
using Microsoft.Win32;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;

namespace Eutyches.Spell.Extensions
{
    /// <summary>
    /// Prompts the user to select a folder with a vista style dialog.
    /// </summary>
    public sealed class VistaFolderBrowserDialog
    {
        #region Properties

        /// <summary>
        /// Gets or sets a valie indicating whether the user is able to select non storage places.
        /// </summary>
        public bool AllowNonStoragePlaces { get; set; }

        /// <summary>
        /// Gets or sets a valie indicating whether the user can select multiple folders or elements.
        /// </summary>
        public bool Multiselect { get; set; }

        /// <summary>
        /// Gets the name of the element selected by the user.
        /// </summary>
        public string SelectedElementName { get; private set; }

        /// <summary>
        /// Gets an array of element names selected by the user.
        /// </summary>
        public string[] SelectedElementNames { get; private set; }

        /// <summary>
        /// Gets or sets the path selected by the user.
        /// </summary>
        public string SelectedPath { get; set; }

        /// <summary>
        /// Gets an array of paths selected by the user.
        /// </summary>
        public string[] SelectedPaths { get; private set; }

        #endregion Properties

        #region Public Methods

        /// <summary>
        /// Shows the dialog with the default owner.
        /// </summary>
        /// <returns><c>true</c> if the user clicks OK in the dialog box; otherwise <c>false</c></returns>
        public bool ShowDialog() => ShowDialog(IntPtr.Zero);

        /// <summary>
        /// Shows the dialog with <paramref name="owner"/> as the owner.
        /// </summary>
        /// <param name="owner">The owner of the dialog box.</param>
        /// <returns><c>true</c> if the user clicks OK in the dialog box; otherwise <c>false</c></returns>
        public bool ShowDialog(Window owner) => ShowDialog(owner == null ? IntPtr.Zero : new WindowInteropHelper(owner).Handle);

        /// <summary>
        /// Shows the dialog with <paramref name="owner"/> as the owner.
        /// </summary>
        /// <param name="owner">The owner of the dialog box.</param>
        /// <returns><c>true</c> if the user clicks OK in the dialog box; otherwise <c>false</c></returns>
        public bool ShowDialog(IWin32Window owner) => ShowDialog(owner == null ? IntPtr.Zero : owner.Handle);

        /// <summary>
        /// Shows the dialog with <paramref name="owner"/> as the owner.
        /// </summary>
        /// <param name="owner">The owner of the dialog box.</param>
        /// <returns><c>true</c> if the user clicks OK in the dialog box; otherwise <c>false</c></returns>
        public bool ShowDialog(IntPtr owner)
        {
            if(Environment.OSVersion.Version.Major < 6)
            {
                throw new InvalidOperationException("The dialog need at least Windows Vista to work.");
            }

            var dialog = CreateNativeDialog();
            try
            {
                SetInitialFolder(dialog);
                SetOptions(dialog);

                if(dialog.Show(owner) != 0)
                {
                    return false;
                }

                SetDialogResults(dialog);

                return true;
            }
            finally
            {
                Marshal.ReleaseComObject(dialog);
            }
        }

        #endregion Public Methods

        #region Helper

        private IFileOpenDialog CreateNativeDialog()
        {
            return new FileOpenDialog() as IFileOpenDialog;
        }

        private FOS GetDialogOptions()
        {
            var options = FOS.PICKFOLDERS;
            if(this.Multiselect)
            {
                options |= FOS.ALLOWMULTISELECT;
            }
            if(!AllowNonStoragePlaces)
            {
                options |= FOS.FORCEFILESYSTEM;
            }
            return options;
        }

        private void GetPathAndElementName(IShellItem item, out string path, out string elementName)
        {
            item.GetDisplayName(SIGDN.PARENTRELATIVEFORADDRESSBAR, out elementName);
            try
            {
                item.GetDisplayName(SIGDN.FILESYSPATH, out path);
            }
            catch(ArgumentException ex) when(ex.HResult == -2147024809)
            {
                path = null;
            }
        }

        private void SetDialogResults(IFileOpenDialog dialog)
        {
            IShellItem item;
            if(!Multiselect)
            {
                dialog.GetResult(out item);
                GetPathAndElementName(item, out string path, out string value);
                this.SelectedPath = path;
                this.SelectedPaths = new[] { path };
                this.SelectedElementName = value;
                this.SelectedElementNames = new[] { value };
            }
            else
            {
                dialog.GetResults(out IShellItemArray items);

                items.GetCount(out uint count);

                this.SelectedPaths = new string[count];
                this.SelectedElementNames = new string[count];

                for(uint i = 0; i < count; ++i)
                {
                    items.GetItemAt(i, out item);
                    GetPathAndElementName(item, out string path, out string value);
                    this.SelectedPaths[i] = path;
                    this.SelectedElementNames[i] = value;
                }

                this.SelectedPath = null;
                this.SelectedElementName = null;
            }
        }

        private void SetInitialFolder(IFileOpenDialog dialog)
        {
            IShellItem item;
            if(!string.IsNullOrEmpty(SelectedPath))
            {
                IntPtr idl;
                uint atts = 0;
                if(NativeMethods.SHILCreateFromPath(SelectedPath, out idl, ref atts) == 0
                    && NativeMethods.SHCreateShellItem(IntPtr.Zero, IntPtr.Zero, idl, out item) == 0)
                {
                    dialog.SetFolder(item);
                }
            }
        }

        private void SetOptions(IFileOpenDialog dialog)
        {
            dialog.SetOptions(GetDialogOptions());
        }

        #endregion Helper

        #region Types

        [Flags]
        private enum FOS
        {
            ALLNONSTORAGEITEMS = 0x80,
            ALLOWMULTISELECT = 0x200,
            CREATEPROMPT = 0x2000,
            DEFAULTNOMINIMODE = 0x20000000,
            DONTADDTORECENT = 0x2000000,
            FILEMUSTEXIST = 0x1000,
            FORCEFILESYSTEM = 0x40,
            FORCESHOWHIDDEN = 0x10000000,
            HIDEMRUPLACES = 0x20000,
            HIDEPINNEDPLACES = 0x40000,
            NOCHANGEDIR = 8,
            NODEREFERENCELINKS = 0x100000,
            NOREADONLYRETURN = 0x8000,
            NOTESTFILECREATE = 0x10000,
            NOVALIDATE = 0x100,
            OVERWRITEPROMPT = 2,
            PATHMUSTEXIST = 0x800,
            PICKFOLDERS = 0x20,
            SHAREAWARE = 0x4000,
            STRICTFILETYPES = 4
        }

        private enum SIGDN : uint
        {
            DESKTOPABSOLUTEEDITING = 0x8004c000,
            DESKTOPABSOLUTEPARSING = 0x80028000,
            FILESYSPATH = 0x80058000,
            NORMALDISPLAY = 0,
            PARENTRELATIVE = 0x80080001,
            PARENTRELATIVEEDITING = 0x80031001,
            PARENTRELATIVEFORADDRESSBAR = 0x8007c001,
            PARENTRELATIVEPARSING = 0x80018001,
            URL = 0x80068000
        }

        [ComImport, Guid("d57c7288-d4ad-4768-be02-9d969532d960"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown), CoClass(typeof(FileOpenDialog))]
        private interface IFileOpenDialog //: IFileDialog
        {
            #region Methods

            void AddPlace([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, FileDialogCustomPlace fdcp);

            void Advise([In, MarshalAs(UnmanagedType.Interface)] IntPtr pfde, out uint pdwCookie);

            void ClearClientData();

            void Close([MarshalAs(UnmanagedType.Error)] int hr);

            void GetCurrentSelection([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

            void GetFileName([MarshalAs(UnmanagedType.LPWStr)] out string pszName);

            void GetFileTypeIndex(out uint piFileType);

            void GetFolder([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

            void GetOptions(out FOS pfos);

            void GetResult([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

            void GetResults([MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppenum);

            void GetSelectedItems([MarshalAs(UnmanagedType.Interface)] out IShellItemArray ppsai);

            void SetClientGuid([In] ref Guid guid);

            void SetDefaultExtension([In, MarshalAs(UnmanagedType.LPWStr)] string pszDefaultExtension);

            void SetDefaultFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

            void SetFileName([In, MarshalAs(UnmanagedType.LPWStr)] string pszName);

            void SetFileNameLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszLabel);

            void SetFileTypeIndex([In] uint iFileType);

            void SetFileTypes([In] uint cFileTypes, [In, MarshalAs(UnmanagedType.Struct)] ref IntPtr rgFilterSpec);

            void SetFilter([MarshalAs(UnmanagedType.Interface)] IntPtr pFilter);

            void SetFolder([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi);

            void SetOkButtonLabel([In, MarshalAs(UnmanagedType.LPWStr)] string pszText);

            void SetOptions([In] FOS fos);

            void SetTitle([In, MarshalAs(UnmanagedType.LPWStr)] string pszTitle);

            [PreserveSig]
            int Show([In] IntPtr parent);

            void Unadvise([In] uint dwCookie);

            #endregion Methods
        }

        [ComImport, Guid("43826D1E-E718-42EE-BC55-A1E261C37BFE"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItem
        {
            #region Methods

            void BindToHandler([In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid bhid, [In] ref Guid riid, out IntPtr ppv);

            void Compare([In, MarshalAs(UnmanagedType.Interface)] IShellItem psi, [In] uint hint, out int piOrder);

            void GetAttributes([In] uint sfgaoMask, out uint psfgaoAttribs);

            void GetDisplayName([In] SIGDN sigdnName, [MarshalAs(UnmanagedType.LPWStr)] out string ppszName);

            void GetParent([MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

            #endregion Methods
        }

        [ComImport, Guid("B63EA76D-1F85-456F-A19C-48159EFA858B"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IShellItemArray
        {
            #region Methods

            void BindToHandler([In, MarshalAs(UnmanagedType.Interface)] IntPtr pbc, [In] ref Guid rbhid, [In] ref Guid riid, out IntPtr ppvOut);

            void EnumItems([MarshalAs(UnmanagedType.Interface)] out IntPtr ppenumShellItems);

            void GetAttributes([In, MarshalAs(UnmanagedType.I4)] IntPtr dwAttribFlags, [In] uint sfgaoMask, out uint psfgaoAttribs);

            void GetCount(out uint pdwNumItems);

            void GetItemAt([In] uint dwIndex, [MarshalAs(UnmanagedType.Interface)] out IShellItem ppsi);

            void GetPropertyDescriptionList([In, MarshalAs(UnmanagedType.Struct)] ref IntPtr keyType, [In] ref Guid riid, out IntPtr ppv);

            void GetPropertyStore([In] int Flags, [In] ref Guid riid, out IntPtr ppv);

            #endregion Methods
        }

        [ComImport, Guid("DC1C5A9C-E88A-4dde-A5A1-60F82A20AEF7")]
        private class FileOpenDialog
        { }

        private class NativeMethods
        {
            #region Methods

            [DllImport("user32.dll")]
            public static extern IntPtr GetActiveWindow();

            [DllImport("shell32.dll")]
            public static extern int SHCreateShellItem(IntPtr pidlParent, IntPtr psfParent, IntPtr pidl, out IShellItem ppsi);

            [DllImport("shell32.dll")]
            public static extern int SHILCreateFromPath([MarshalAs(UnmanagedType.LPWStr)] string pszPath, out IntPtr ppIdl, ref uint rgflnOut);

            #endregion Methods
        }

        #endregion Types
    }
}