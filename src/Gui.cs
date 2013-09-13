/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
//
// TGUI - Texus's Graphical User Interface
// Copyright (C) 2012-2013 Bruno Van de Velde (vdv_b@tgui.eu)
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
using System.Collections.Generic;
using System.Resources;
using SFML.Window;
using SFML.Graphics;
using Tao.OpenGl;

namespace TGUI
{
    public class Gui
    {
        private RenderWindow m_Window;

        private int m_StartTime = Environment.TickCount;

        private GuiContainer m_Container = new GuiContainer();

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Gui (RenderWindow window)
        {
            m_Window = window;
            m_Container.m_Window = window;
            m_Container.m_ContainerFocused = true;

            window.MouseMoved += new EventHandler<MouseMoveEventArgs>(OnMouseMoved);
            window.MouseButtonPressed += new EventHandler<MouseButtonEventArgs>(OnMousePressed);
            window.MouseButtonReleased += new EventHandler<MouseButtonEventArgs>(OnMouseReleased);
            window.KeyPressed += new EventHandler<KeyEventArgs>(m_Container.m_EventManager.OnKeyPressed);
            window.KeyReleased += new EventHandler<KeyEventArgs>(m_Container.m_EventManager.OnKeyReleased);
            window.TextEntered += new EventHandler<TextEventArgs>(m_Container.m_EventManager.OnTextEntered);
            window.MouseWheelMoved += new EventHandler<MouseWheelEventArgs>(OnMouseWheelMoved);        
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Gui(RenderWindow window, ResourceManager manager) :
            this(window)
        {
            Global.ResourceManager = manager;
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Draw ()
        {
            // Update the time
            int currentTime = Environment.TickCount;
            m_Container.m_EventManager.UpdateTime (currentTime - m_StartTime);
            m_StartTime = currentTime;

            // Check if clipping is enabled
            int clippingEnabled = Gl.glIsEnabled(Gl.GL_SCISSOR_TEST);
            int[] scissor;
            scissor = new int[4];

            if (clippingEnabled != 0)
            {
                // Remember the old clipping area
                Gl.glGetIntegerv(Gl.GL_SCISSOR_BOX, scissor);
            }
            else // Clipping was disabled
            {
                // Enable clipping
                Gl.glEnable(Gl.GL_SCISSOR_TEST);
                Gl.glScissor(0, 0, (int)m_Window.Size.X, (int)m_Window.Size.Y);
            }

            // Draw the window with all widgets inside it
            m_Container.DrawContainer(m_Window, SFML.Graphics.RenderStates.Default);

            // Reset clipping to its original state
            if (clippingEnabled != 0)
                Gl.glScissor(scissor[0], scissor[1], scissor[2], scissor[3]);
            else
                Gl.glDisable(Gl.GL_SCISSOR_TEST);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public RenderWindow Window
        {
            get
            {
                return m_Window;
            }
            set
            {
                m_Window = value;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public Font GlobalFont
        {
            get
            {
                return m_Container.GlobalFont;
            }
            set
            {
                m_Container.GlobalFont = value;
            }
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<string> GetWidgetNames()
        {
            return m_Container.GetWidgetNames();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public List<Widget> GetWidgets()
        {
            return m_Container.GetWidgets();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public T Add<T> (T widget, string name = "") where T : Widget
        {
            return m_Container.Add (widget, name);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public T Get<T> (string name) where T : Widget
        {
            return m_Container.Get<T> (name);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Remove (Widget widget)
        {
            m_Container.Remove (widget);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void Remove (string name)
        {
            m_Container.Remove (name);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void RemoveAllWidgets ()
        {
            m_Container.RemoveAllWidgets ();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool SetWidgetName(Widget widget, string name)
        {
            return m_Container.SetWidgetName(widget, name);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public bool GetWidgetName(Widget widget, ref string name)
        {
            return m_Container.GetWidgetName(widget, ref name);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void FocusWidget (Widget widget)
        {
            m_Container.FocusWidget (widget);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \brief Focuses the next widget.
        ///
        /// The currently focused widget will be unfocused, even if it was the only widget.
        /// When no widget was focused, the first widget in the container will be focused.
        ///
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void FocusNextWidget ()
        {
            m_Container.FocusNextWidget ();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// \brief Focuses the previous widget.
        ///
        /// The currently focused widget will be unfocused, even if it was the only widget.
        /// When no widget was focused, the last widget in the container will be focused.
        ///
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void FocusPreviousWidget ()
        {
            m_Container.FocusPreviousWidget ();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UnfocusWidgets ()
        {
            m_Container.UnfocusWidgets ();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void UncheckRadioButtons ()
        {
            m_Container.UncheckRadioButtons ();
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void MoveWidgetToFront (Widget widget)
        {
            m_Container.MoveWidgetToFront (widget);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        public void MoveWidgetToBack (Widget widget)
        {
            m_Container.MoveWidgetToBack (widget);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnMouseMoved (object sender, MouseMoveEventArgs e)
        {
            Vector2f mouseCoords = m_Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            e.X = (int)mouseCoords.X;
            e.Y = (int)mouseCoords.Y;

            m_Container.m_EventManager.OnMouseMoved (sender, e);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnMousePressed (object sender, MouseButtonEventArgs e)
        {
            Vector2f mouseCoords = m_Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            e.X = (int)mouseCoords.X;
            e.Y = (int)mouseCoords.Y;

            m_Container.m_EventManager.OnMousePressed (sender, e);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnMouseReleased (object sender, MouseButtonEventArgs e)
        {
            Vector2f mouseCoords = m_Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            e.X = (int)mouseCoords.X;
            e.Y = (int)mouseCoords.Y;

            m_Container.m_EventManager.OnMouseReleased (sender, e);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        private void OnMouseWheelMoved (object sender, MouseWheelEventArgs e)
        {
            Vector2f mouseCoords = m_Window.MapPixelToCoords(new Vector2i(e.X, e.Y));
            e.X = (int)mouseCoords.X;
            e.Y = (int)mouseCoords.Y;

            m_Container.m_EventManager.OnMouseWheelMoved (sender, e);
        }


        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
    }
}

