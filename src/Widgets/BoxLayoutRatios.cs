/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// TGUI - Texus' Graphical User Interface
// Copyright (C) 2012-2016 Bruno Van de Velde (vdv_b@tgui.eu)
//
// This software is provided 'as-is', without any express or implied warranty.
// In no event will the authors be held liable for any damages arising from the use of this software.
//
// Permission is granted to anyone to use this software for any purpose,
// including commercial applications, and to alter it and redistribute it freely,
// subject to the following restrictions:
//
// 1. The origin of this software must not be misrepresented;
//    you must not claim that you wrote the original software.
//    If you use this software in a product, an acknowledgment
//    in the product documentation would be appreciated but is not required.
//
// 2. Altered source versions must be plainly marked as such,
//    and must not be misrepresented as being the original software.
//
// 3. This notice may not be removed or altered from any source distribution.
//
/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Security;
using System.Runtime.InteropServices;

namespace TGUI
{
    public abstract class BoxLayoutRatios : BoxLayout
    {
        protected internal BoxLayoutRatios(IntPtr cPointer)
            : base(cPointer)
        {
        }

        public BoxLayoutRatios(BoxLayoutRatios copy)
            : base(copy)
        {
        }

        public void Add(Widget widget, float ratio, string widgetName = "")
        {
            tguiBoxLayoutRatios_add(CPointer, widget.CPointer, ratio, Util.ConvertStringForC_UTF32(widgetName));

            widget.ParentGui = ParentGui;
            myWidgets.Add(widget);
            myWidgetIds.Add(widgetName);
        }

        public void Insert(uint index, Widget widget, float ratio, string widgetName = "")
        {
            tguiBoxLayoutRatios_insert(CPointer, index, widget.CPointer, ratio, Util.ConvertStringForC_UTF32(widgetName));
        }

        public void AddSpace(float ratio)
        {
            tguiBoxLayoutRatios_addSpace(CPointer, ratio);
        }

        public void InsertSpace(uint index, float ratio)
        {
            tguiBoxLayoutRatios_insertSpace(CPointer, index, ratio);
        }

        public void SetRatio(Widget widget, float ratio)
        {
            tguiBoxLayoutRatios_setRatio(CPointer, widget.CPointer, ratio);
        }

        public void SetRatio(uint index, float ratio)
        {
            tguiBoxLayoutRatios_setRatioAtIndex(CPointer, index, ratio);
        }

        public float GetRatio(Widget widget)
        {
            return tguiBoxLayoutRatios_getRatio(CPointer, widget.CPointer);
        }

        public float GetRatio(uint index)
        {
            return tguiBoxLayoutRatios_getRatioAtIndex(CPointer, index);
        }


        #region Imports

        [DllImport(Global.CTGUI, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        static extern protected void tguiBoxLayoutRatios_add(IntPtr cPointer, IntPtr widgetCPointer, float ratio, IntPtr widgetName);

        [DllImport(Global.CTGUI, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        static extern protected void tguiBoxLayoutRatios_insert(IntPtr cPointer, uint index, IntPtr widgetCPointer, float ratio, IntPtr widgetName);

        [DllImport(Global.CTGUI, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        static extern protected void tguiBoxLayoutRatios_addSpace(IntPtr cPointer, float ratio);

        [DllImport(Global.CTGUI, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        static extern protected void tguiBoxLayoutRatios_insertSpace(IntPtr cPointer, uint index, float ratio);

        [DllImport(Global.CTGUI, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        static extern protected void tguiBoxLayoutRatios_setRatio(IntPtr cPointer, IntPtr widgetCPointer, float ratio);

        [DllImport(Global.CTGUI, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        static extern protected void tguiBoxLayoutRatios_setRatioAtIndex(IntPtr cPointer, uint index, float ratio);

        [DllImport(Global.CTGUI, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        static extern protected float tguiBoxLayoutRatios_getRatio(IntPtr cPointer, IntPtr widgetCPointer);

        [DllImport(Global.CTGUI, CallingConvention = CallingConvention.Cdecl), SuppressUnmanagedCodeSecurity]
        static extern protected float tguiBoxLayoutRatios_getRatioAtIndex(IntPtr cPointer, uint index);

        #endregion
    }
}
