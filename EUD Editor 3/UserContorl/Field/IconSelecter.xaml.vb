﻿Imports MaterialDesignThemes.Wpf

Public Class IconSelecter
    Private DatFile As SCDatFiles.DatFiles
    Private ObjectID As Integer
    Private Parameter As String

    Private DatCommand As DatCommand

    Public Sub Init(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String, Optional TextWidth As Integer = 0)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        If _Parameter = "UnitName" Then
            IconBox.Visibility = Visibility.Collapsed
            Field.Init(DatFile, ObjectID, Parameter, InputField.SFlag.None, TextWidth)
            btn.Background = New SolidColorBrush(pgData.FiledDefault)
            btn.Content = pjData.Stat_txt(ObjectID)
            btn.ContextMenu = Nothing

            Dim strindex As Integer = 0
            If pjData.IsMapLoading Then
                strindex = pjData.MapData.DatFile.Data(SCDatFiles.DatFiles.units, "Unit Map String", ObjectID)
            End If


            If strindex <> 0 Then
                btn.IsEnabled = False
                OpenNew.IsEnabled = False
            Else
                btn.IsEnabled = True
                OpenNew.IsEnabled = True
            End If
        Else
            Field.Init(DatFile, ObjectID, Parameter, InputField.SFlag.None, TextWidth)

            Dim valueType As SCDatFiles.DatFiles = pjData.Dat.ParamInfo(DatFile, Parameter, SCDatFiles.EParamInfo.ValueType)
            If valueType = SCDatFiles.DatFiles.sfxdata Or valueType = SCDatFiles.DatFiles.portdata Or valueType = SCDatFiles.DatFiles.IscriptID Then
                IconBox.Visibility = Visibility.Collapsed
                OpenNew.Visibility = Visibility.Collapsed
            End If
            If valueType = SCDatFiles.DatFiles.stattxt Then
                IconBox.Visibility = Visibility.Collapsed
            End If

            If pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID) IsNot Nothing Then

                DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)
                btn.IsEnabled = True
                IconImage.IsEnabled = True
                If valueType = SCDatFiles.DatFiles.Icon Then
                    OpenNew.Visibility = Visibility.Collapsed
                Else
                    OpenNew.IsEnabled = True
                End If


                Dim tBinding1 As New Binding
                tBinding1.Path = New PropertyPath("ValueText")
                'tBinding1.Source = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).ValueTextBinding
                btn.SetBinding(Button.ContentProperty, tBinding1)

                Dim tBinding2 As New Binding
                tBinding2.Path = New PropertyPath("ValueImage")
                IconImage.SetBinding(Image.SourceProperty, tBinding2)

                Dim tBinding3 As New Binding
                tBinding3.Path = New PropertyPath("BackColor")
                btn.SetBinding(Button.BackgroundProperty, tBinding3)
            Else
                btn.Content = Tool.GetText("NotUse")
                IconImage.Source = New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/BlackIcon.png"))
                IconImage.IsEnabled = False
                btn.IsEnabled = False
                OpenNew.IsEnabled = False
                Exit Sub
            End If

            DatCommand = New DatCommand(DatFile, Parameter, ObjectID)




            CopyItem.Command = DatCommand
            CopyItem.CommandParameter = DatCommand.CommandType.Copy

            PasteItem.Command = DatCommand
            PasteItem.CommandParameter = DatCommand.CommandType.Paste

            ResetItem.Command = DatCommand
            ResetItem.CommandParameter = DatCommand.CommandType.Reset

        End If


    End Sub
    Public Sub ReLoad(_DatFile As SCDatFiles.DatFiles, _ObjectID As Integer, _Parameter As String)
        DatFile = _DatFile
        ObjectID = _ObjectID
        Parameter = _Parameter

        If _Parameter = "UnitName" Then
            Field.ReLoad(DatFile, ObjectID, Parameter)
            IconBox.Visibility = Visibility.Collapsed
            btn.Content = pjData.Stat_txt(ObjectID)
            Dim strindex As Integer = 0
            If pjData.IsMapLoading Then
                strindex = pjData.MapData.DatFile.Data(SCDatFiles.DatFiles.units, "Unit Map String", ObjectID)
            End If

            If strindex <> 0 Then
                btn.IsEnabled = False
                OpenNew.IsEnabled = False
            Else
                btn.IsEnabled = True
                OpenNew.IsEnabled = True
            End If
        Else
            Field.ReLoad(DatFile, ObjectID, Parameter)

            If pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID) IsNot Nothing Then
                DataContext = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID)

                btn.IsEnabled = True
                IconImage.IsEnabled = True
                OpenNew.IsEnabled = True

                Dim tBinding1 As New Binding
                tBinding1.Path = New PropertyPath("ValueText")
                'tBinding1.Source = pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).ValueTextBinding
                btn.SetBinding(Button.ContentProperty, tBinding1)

                Dim tBinding2 As New Binding
                tBinding2.Path = New PropertyPath("ValueImage")
                IconImage.SetBinding(Image.SourceProperty, tBinding2)

                Dim tBinding3 As New Binding
                tBinding3.Path = New PropertyPath("BackColor")
                btn.SetBinding(Button.BackgroundProperty, tBinding3)
            Else
                btn.Content = Tool.GetText("NotUse")
                IconImage.Source = New BitmapImage(New Uri("pack://siteoforigin:,,,/Data/Resources/BlackIcon.png"))
                IconImage.IsEnabled = False
                btn.IsEnabled = False
                OpenNew.IsEnabled = False
                Exit Sub
            End If

            If DatCommand Is Nothing Then
                DatCommand = New DatCommand(DatFile, Parameter, ObjectID)

                CopyItem.Command = DatCommand
                CopyItem.CommandParameter = DatCommand.CommandType.Copy

                PasteItem.Command = DatCommand
                PasteItem.CommandParameter = DatCommand.CommandType.Paste

                ResetItem.Command = DatCommand
                ResetItem.CommandParameter = DatCommand.CommandType.Reset
            Else
                DatCommand.ReLoad(DatFile, Parameter, ObjectID)
            End If
        End If
    End Sub


    Private Sub Click(sender As Object, e As RoutedEventArgs) Handles btn.Click
        If Parameter = "UnitName" Then
            TabItemTool.WindowTabItem(SCDatFiles.DatFiles.stattxt, ObjectID)
        Else
            'CodeList.SetFliter(CodeSelecter.ESortType.n123)
            Dim valueType As SCDatFiles.DatFiles = pjData.Dat.ParamInfo(DatFile, Parameter, SCDatFiles.EParamInfo.ValueType)
            If valueType <> SCDatFiles.DatFiles.None Then
                If Not CodeList.DataLoadCmp Then
                    CodeList.ListReset(valueType, True, pjData.Dat.Data(DatFile, Parameter, ObjectID))
                Else
                    CodeList.Refresh(pjData.Dat.Data(DatFile, Parameter, ObjectID))
                End If

                CodeList.MaxWidth = btn.RenderSize.Width + 32
                CodeList.Width = btn.RenderSize.Width + 32
                CodeSelect.IsOpen = True
            End If
        End If
    End Sub

    Private Sub CodeList_Select(Code As Object, e As RoutedEventArgs)
        CodeSelect.IsOpen = False
        Field.ValueText.Text = Code(1)
        'MsgBox(Code(1))
    End Sub
    Private Sub OpneMenu(sender As Object, e As ContextMenuEventArgs) Handles btn.ContextMenuOpening
        If Parameter <> "UnitName" Then
            CopyItem.IsEnabled = DatCommand.IsEnabled(CopyItem.CommandParameter)
            PasteItem.IsEnabled = DatCommand.IsEnabled(PasteItem.CommandParameter)
            ResetItem.IsEnabled = DatCommand.IsEnabled(ResetItem.CommandParameter)
        End If
    End Sub

    Private Sub btnMouseWhell(sender As Object, e As MouseWheelEventArgs) Handles btn.MouseWheel
        If Parameter <> "UnitName" Then
            Dim ChangeValue As Integer = e.Delta / 100
            pjData.BindingManager.DatBinding(DatFile, Parameter, ObjectID).Value += ChangeValue
        End If
    End Sub

    Private Sub OpenNew_Click(sender As Object, e As RoutedEventArgs) Handles OpenNew.Click
        If Parameter = "UnitName" Then
            TabItemTool.WindowTabItem(SCDatFiles.DatFiles.stattxt, ObjectID)
        Else
            Dim valueType As SCDatFiles.DatFiles = pjData.Dat.ParamInfo(DatFile, Parameter, SCDatFiles.EParamInfo.ValueType)
            Dim value As Integer = pjData.Dat.Data(DatFile, Parameter, ObjectID)
            If CheckOverFlow(valueType, value) Then
                TabItemTool.WindowTabItem(valueType, value)
            End If
        End If

    End Sub
End Class
