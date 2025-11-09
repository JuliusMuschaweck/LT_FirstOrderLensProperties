<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        components = New ComponentModel.Container()
        Button1 = New Button()
        CheckBoxDrawFocalPoints = New CheckBox()
        CheckBoxDrawPrincipalPoints = New CheckBox()
        CheckBoxDrawFocalPlanes = New CheckBox()
        CheckBoxDrawPrincipalPlanes = New CheckBox()
        FocalPlaneHalfWidthTextBox = New TextBox()
        PrincipalPlaneHalfWidthTextBox = New TextBox()
        ErrorProvider1 = New ErrorProvider(components)
        cboSolids = New ComboBox()
        cboSolidsClone = New ComboBox()
        btnRefresh = New Button()
        lblDescription = New Label()
        Label1 = New Label()
        Label2 = New Label()
        GroupBox1 = New GroupBox()
        txtLog = New TextBox()
        PictureBox1 = New PictureBox()
        chkUseLTSelection = New CheckBox()
        LinkLabel1 = New LinkLabel()
        btnSampleLens = New Button()
        CType(ErrorProvider1, ComponentModel.ISupportInitialize).BeginInit()
        GroupBox1.SuspendLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        Button1.AutoSize = True
        Button1.AutoSizeMode = AutoSizeMode.GrowAndShrink
        Button1.Font = New Font("Microsoft Sans Serif", 16.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Button1.Location = New Point(332, 28)
        Button1.Name = "Button1"
        Button1.Size = New Size(276, 42)
        Button1.TabIndex = 0
        Button1.Text = "Compute Properties"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDrawFocalPoints
        ' 
        CheckBoxDrawFocalPoints.AutoSize = True
        CheckBoxDrawFocalPoints.Font = New Font("Microsoft Sans Serif", 9F)
        CheckBoxDrawFocalPoints.Location = New Point(7, 28)
        CheckBoxDrawFocalPoints.Name = "CheckBoxDrawFocalPoints"
        CheckBoxDrawFocalPoints.Size = New Size(145, 22)
        CheckBoxDrawFocalPoints.TabIndex = 3
        CheckBoxDrawFocalPoints.Text = "Draw focal points"
        CheckBoxDrawFocalPoints.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDrawPrincipalPoints
        ' 
        CheckBoxDrawPrincipalPoints.AutoSize = True
        CheckBoxDrawPrincipalPoints.Font = New Font("Microsoft Sans Serif", 9F)
        CheckBoxDrawPrincipalPoints.Location = New Point(7, 63)
        CheckBoxDrawPrincipalPoints.Name = "CheckBoxDrawPrincipalPoints"
        CheckBoxDrawPrincipalPoints.Size = New Size(167, 22)
        CheckBoxDrawPrincipalPoints.TabIndex = 4
        CheckBoxDrawPrincipalPoints.Text = "Draw principal points"
        CheckBoxDrawPrincipalPoints.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDrawFocalPlanes
        ' 
        CheckBoxDrawFocalPlanes.AutoSize = True
        CheckBoxDrawFocalPlanes.Font = New Font("Microsoft Sans Serif", 9F)
        CheckBoxDrawFocalPlanes.Location = New Point(7, 98)
        CheckBoxDrawFocalPlanes.Name = "CheckBoxDrawFocalPlanes"
        CheckBoxDrawFocalPlanes.Size = New Size(241, 22)
        CheckBoxDrawFocalPlanes.TabIndex = 5
        CheckBoxDrawFocalPlanes.Text = "Draw focal planes. Half Width = "
        CheckBoxDrawFocalPlanes.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDrawPrincipalPlanes
        ' 
        CheckBoxDrawPrincipalPlanes.AutoSize = True
        CheckBoxDrawPrincipalPlanes.Font = New Font("Microsoft Sans Serif", 9F)
        CheckBoxDrawPrincipalPlanes.Location = New Point(6, 133)
        CheckBoxDrawPrincipalPlanes.Name = "CheckBoxDrawPrincipalPlanes"
        CheckBoxDrawPrincipalPlanes.Size = New Size(263, 22)
        CheckBoxDrawPrincipalPlanes.TabIndex = 6
        CheckBoxDrawPrincipalPlanes.Text = "Draw principal planes. Half Width = "
        CheckBoxDrawPrincipalPlanes.TextAlign = ContentAlignment.TopRight
        CheckBoxDrawPrincipalPlanes.UseVisualStyleBackColor = True
        ' 
        ' FocalPlaneHalfWidthTextBox
        ' 
        FocalPlaneHalfWidthTextBox.Font = New Font("Microsoft Sans Serif", 9F)
        FocalPlaneHalfWidthTextBox.Location = New Point(272, 92)
        FocalPlaneHalfWidthTextBox.Name = "FocalPlaneHalfWidthTextBox"
        FocalPlaneHalfWidthTextBox.Size = New Size(39, 24)
        FocalPlaneHalfWidthTextBox.TabIndex = 7
        FocalPlaneHalfWidthTextBox.Text = "5"
        FocalPlaneHalfWidthTextBox.TextAlign = HorizontalAlignment.Right
        ' 
        ' PrincipalPlaneHalfWidthTextBox
        ' 
        PrincipalPlaneHalfWidthTextBox.Font = New Font("Microsoft Sans Serif", 9F)
        PrincipalPlaneHalfWidthTextBox.Location = New Point(272, 131)
        PrincipalPlaneHalfWidthTextBox.Name = "PrincipalPlaneHalfWidthTextBox"
        PrincipalPlaneHalfWidthTextBox.Size = New Size(39, 24)
        PrincipalPlaneHalfWidthTextBox.TabIndex = 8
        PrincipalPlaneHalfWidthTextBox.Text = "5"
        PrincipalPlaneHalfWidthTextBox.TextAlign = HorizontalAlignment.Right
        ' 
        ' ErrorProvider1
        ' 
        ErrorProvider1.ContainerControl = Me
        ' 
        ' cboSolids
        ' 
        cboSolids.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        cboSolids.FormattingEnabled = True
        cboSolids.Location = New Point(125, 128)
        cboSolids.Margin = New Padding(3, 4, 3, 4)
        cboSolids.Name = "cboSolids"
        cboSolids.Size = New Size(323, 28)
        cboSolids.TabIndex = 9
        ' 
        ' cboSolidsClone
        ' 
        cboSolidsClone.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        cboSolidsClone.FormattingEnabled = True
        cboSolidsClone.Location = New Point(125, 167)
        cboSolidsClone.Margin = New Padding(3, 4, 3, 4)
        cboSolidsClone.Name = "cboSolidsClone"
        cboSolidsClone.Size = New Size(323, 28)
        cboSolidsClone.TabIndex = 10
        ' 
        ' btnRefresh
        ' 
        btnRefresh.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnRefresh.Location = New Point(299, 89)
        btnRefresh.Margin = New Padding(3, 4, 3, 4)
        btnRefresh.Name = "btnRefresh"
        btnRefresh.Size = New Size(150, 31)
        btnRefresh.TabIndex = 11
        btnRefresh.Text = "Refresh surface lists"
        btnRefresh.UseVisualStyleBackColor = True
        ' 
        ' lblDescription
        ' 
        lblDescription.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        lblDescription.Location = New Point(14, 12)
        lblDescription.Name = "lblDescription"
        lblDescription.Size = New Size(435, 73)
        lblDescription.TabIndex = 12
        lblDescription.Text = "Select..."
        ' 
        ' Label1
        ' 
        Label1.AutoSize = True
        Label1.Location = New Point(22, 132)
        Label1.Name = "Label1"
        Label1.Size = New Size(89, 20)
        Label1.TabIndex = 13
        Label1.Text = "First Surface"
        ' 
        ' Label2
        ' 
        Label2.AutoSize = True
        Label2.Location = New Point(22, 171)
        Label2.Name = "Label2"
        Label2.Size = New Size(88, 20)
        Label2.TabIndex = 13
        Label2.Text = "Last Surface"
        ' 
        ' GroupBox1
        ' 
        GroupBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Right
        GroupBox1.Controls.Add(CheckBoxDrawFocalPoints)
        GroupBox1.Controls.Add(CheckBoxDrawPrincipalPoints)
        GroupBox1.Controls.Add(CheckBoxDrawFocalPlanes)
        GroupBox1.Controls.Add(CheckBoxDrawPrincipalPlanes)
        GroupBox1.Controls.Add(FocalPlaneHalfWidthTextBox)
        GroupBox1.Controls.Add(PrincipalPlaneHalfWidthTextBox)
        GroupBox1.Controls.Add(Button1)
        GroupBox1.Location = New Point(22, 239)
        GroupBox1.Margin = New Padding(3, 4, 3, 4)
        GroupBox1.Name = "GroupBox1"
        GroupBox1.Padding = New Padding(3, 4, 3, 4)
        GroupBox1.Size = New Size(614, 180)
        GroupBox1.TabIndex = 14
        GroupBox1.TabStop = False
        GroupBox1.Text = "Compute First Order Properties"
        ' 
        ' txtLog
        ' 
        txtLog.Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
        txtLog.Font = New Font("Courier New", 10.2F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        txtLog.Location = New Point(22, 427)
        txtLog.Margin = New Padding(3, 4, 3, 4)
        txtLog.Multiline = True
        txtLog.Name = "txtLog"
        txtLog.ReadOnly = True
        txtLog.Size = New Size(607, 244)
        txtLog.TabIndex = 15
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        PictureBox1.Location = New Point(477, 13)
        PictureBox1.Margin = New Padding(3, 4, 3, 4)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(152, 130)
        PictureBox1.SizeMode = PictureBoxSizeMode.AutoSize
        PictureBox1.TabIndex = 16
        PictureBox1.TabStop = False
        ' 
        ' chkUseLTSelection
        ' 
        chkUseLTSelection.AutoSize = True
        chkUseLTSelection.Checked = True
        chkUseLTSelection.CheckState = CheckState.Checked
        chkUseLTSelection.Location = New Point(22, 205)
        chkUseLTSelection.Margin = New Padding(3, 4, 3, 4)
        chkUseLTSelection.Name = "chkUseLTSelection"
        chkUseLTSelection.Size = New Size(192, 24)
        chkUseLTSelection.TabIndex = 17
        chkUseLTSelection.Text = "Use LightTools Selection"
        chkUseLTSelection.UseVisualStyleBackColor = True
        ' 
        ' LinkLabel1
        ' 
        LinkLabel1.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        LinkLabel1.AutoSize = True
        LinkLabel1.Location = New Point(469, 147)
        LinkLabel1.Name = "LinkLabel1"
        LinkLabel1.Size = New Size(167, 20)
        LinkLabel1.TabIndex = 18
        LinkLabel1.TabStop = True
        LinkLabel1.Text = "JMO Illumination Optics"
        ' 
        ' btnSampleLens
        ' 
        btnSampleLens.Anchor = AnchorStyles.Top Or AnchorStyles.Right
        btnSampleLens.Location = New Point(125, 89)
        btnSampleLens.Margin = New Padding(3, 4, 3, 4)
        btnSampleLens.Name = "btnSampleLens"
        btnSampleLens.Size = New Size(168, 31)
        btnSampleLens.TabIndex = 19
        btnSampleLens.Text = "Insert demo lens"
        btnSampleLens.UseVisualStyleBackColor = True
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(643, 688)
        Controls.Add(btnSampleLens)
        Controls.Add(LinkLabel1)
        Controls.Add(chkUseLTSelection)
        Controls.Add(PictureBox1)
        Controls.Add(txtLog)
        Controls.Add(GroupBox1)
        Controls.Add(Label2)
        Controls.Add(Label1)
        Controls.Add(lblDescription)
        Controls.Add(btnRefresh)
        Controls.Add(cboSolidsClone)
        Controls.Add(cboSolids)
        Name = "Form1"
        Text = "LightTools first order lens properties"
        CType(ErrorProvider1, ComponentModel.ISupportInitialize).EndInit()
        GroupBox1.ResumeLayout(False)
        GroupBox1.PerformLayout()
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents CheckBoxDrawFocalPoints As CheckBox
    Friend WithEvents CheckBoxDrawPrincipalPoints As CheckBox
    Friend WithEvents CheckBoxDrawFocalPlanes As CheckBox
    Friend WithEvents CheckBoxDrawPrincipalPlanes As CheckBox
    Friend WithEvents FocalPlaneHalfWidthTextBox As TextBox
    Friend WithEvents PrincipalPlaneHalfWidthTextBox As TextBox
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents cboSolidsClone As ComboBox
    Friend WithEvents cboSolids As ComboBox
    Friend WithEvents btnRefresh As Button
    Friend WithEvents lblDescription As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtLog As TextBox
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents chkUseLTSelection As CheckBox
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents btnSampleLens As Button

End Class
