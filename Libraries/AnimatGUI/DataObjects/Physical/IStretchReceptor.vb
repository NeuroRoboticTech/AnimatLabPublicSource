Imports System
Imports System.Drawing
Imports System.Collections
Imports System.ComponentModel
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.IO
Imports System.Xml
Imports AnimatGuiCtrls.Controls
Imports AnimatGUI.Framework

Namespace DataObjects.Physical

    Public Interface IStretchReceptor

        Property IaDischargeConstant() As ScaledNumber
        Property IIDischargeConstant() As ScaledNumber

    End Interface

End Namespace
