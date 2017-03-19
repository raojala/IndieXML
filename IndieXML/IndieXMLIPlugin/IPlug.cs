﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace IndieXMLIPlugin
{
    /// <summary>
    /// IPlug is an interface for IndieXML plugins, that all plugins must implement
    /// in order to be recognized and loaded.
    /// </summary>
    public interface IPlug
    {
        string Name { get; } // name works as a keyvalue in dictionary, must be unique!
        void Update(StackPanel TopNav, StackPanel BotNav, StackPanel MainContent); // method that gets called in pluginmanager.
    }
}