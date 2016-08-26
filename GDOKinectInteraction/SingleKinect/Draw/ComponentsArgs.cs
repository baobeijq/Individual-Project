﻿using System.Windows.Controls;

namespace SingleKinect.Draw
{
    public class ComponentsArgs
    {
        private object[] components;

        internal object[] Components => components;

        public ComponentsArgs(Label leftLabel, Label rightLabel, Canvas bodyCanvas, Canvas engagerCanvas, Canvas transformCanvas, Label faceLabel)
        {
            components = new object[6];//was[5]
            components[0] = leftLabel;
            components[1] = rightLabel;
            components[2] = bodyCanvas;
            components[3] = engagerCanvas;
            components[4] = transformCanvas;//new added
            components[5] = faceLabel;

        }
    }
}