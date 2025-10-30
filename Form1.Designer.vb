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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Button1 = New Button()
        PictureBox1 = New PictureBox()
        LinkLabel1 = New LinkLabel()
        CheckBoxDrawFocalPoints = New CheckBox()
        CheckBoxDrawPrincipalPoints = New CheckBox()
        CheckBoxDrawFocalPlanes = New CheckBox()
        CheckBoxDrawPrincipalPlanes = New CheckBox()
        FocalPlaneHalfWidthTextBox = New TextBox()
        PrincipalPlaneHalfWidthTextBox = New TextBox()
        ErrorProvider1 = New ErrorProvider(components)
        CType(PictureBox1, ComponentModel.ISupportInitialize).BeginInit()
        CType(ErrorProvider1, ComponentModel.ISupportInitialize).BeginInit()
        SuspendLayout()
        ' 
        ' Button1
        ' 
        Button1.Font = New Font("Segoe UI", 13.8F, FontStyle.Regular, GraphicsUnit.Point, CByte(0))
        Button1.Location = New Point(12, 12)
        Button1.Name = "Button1"
        Button1.Size = New Size(268, 87)
        Button1.TabIndex = 0
        Button1.Text = "Compute lens properties"
        Button1.UseVisualStyleBackColor = True
        ' 
        ' PictureBox1
        ' 
        PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), Image)
        PictureBox1.Location = New Point(380, 12)
        PictureBox1.Name = "PictureBox1"
        PictureBox1.Size = New Size(130, 129)
        PictureBox1.TabIndex = 1
        PictureBox1.TabStop = False
        ' 
        ' LinkLabel1
        ' 
        LinkLabel1.AutoSize = True
        LinkLabel1.Location = New Point(362, 144)
        LinkLabel1.Name = "LinkLabel1"
        LinkLabel1.Size = New Size(170, 20)
        LinkLabel1.TabIndex = 2
        LinkLabel1.TabStop = True
        LinkLabel1.Text = "https://www.jmoptics.de"
        ' 
        ' CheckBoxDrawFocalPoints
        ' 
        CheckBoxDrawFocalPoints.AutoSize = True
        CheckBoxDrawFocalPoints.Font = New Font("Segoe UI", 12F)
        CheckBoxDrawFocalPoints.Location = New Point(12, 105)
        CheckBoxDrawFocalPoints.Name = "CheckBoxDrawFocalPoints"
        CheckBoxDrawFocalPoints.Size = New Size(186, 32)
        CheckBoxDrawFocalPoints.TabIndex = 3
        CheckBoxDrawFocalPoints.Text = "Draw focal points"
        CheckBoxDrawFocalPoints.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDrawPrincipalPoints
        ' 
        CheckBoxDrawPrincipalPoints.AutoSize = True
        CheckBoxDrawPrincipalPoints.Font = New Font("Segoe UI", 12F)
        CheckBoxDrawPrincipalPoints.Location = New Point(12, 144)
        CheckBoxDrawPrincipalPoints.Name = "CheckBoxDrawPrincipalPoints"
        CheckBoxDrawPrincipalPoints.Size = New Size(220, 32)
        CheckBoxDrawPrincipalPoints.TabIndex = 4
        CheckBoxDrawPrincipalPoints.Text = "Draw principal points"
        CheckBoxDrawPrincipalPoints.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDrawFocalPlanes
        ' 
        CheckBoxDrawFocalPlanes.AutoSize = True
        CheckBoxDrawFocalPlanes.Font = New Font("Segoe UI", 12F)
        CheckBoxDrawFocalPlanes.Location = New Point(12, 182)
        CheckBoxDrawFocalPlanes.Name = "CheckBoxDrawFocalPlanes"
        CheckBoxDrawFocalPlanes.Size = New Size(282, 32)
        CheckBoxDrawFocalPlanes.TabIndex = 5
        CheckBoxDrawFocalPlanes.Text = "Draw focal planes, half width"
        CheckBoxDrawFocalPlanes.UseVisualStyleBackColor = True
        ' 
        ' CheckBoxDrawPrincipalPlanes
        ' 
        CheckBoxDrawPrincipalPlanes.AutoSize = True
        CheckBoxDrawPrincipalPlanes.Font = New Font("Segoe UI", 12F)
        CheckBoxDrawPrincipalPlanes.Location = New Point(12, 220)
        CheckBoxDrawPrincipalPlanes.Name = "CheckBoxDrawPrincipalPlanes"
        CheckBoxDrawPrincipalPlanes.Size = New Size(316, 32)
        CheckBoxDrawPrincipalPlanes.TabIndex = 6
        CheckBoxDrawPrincipalPlanes.Text = "Draw principal planes, half width"
        CheckBoxDrawPrincipalPlanes.TextAlign = ContentAlignment.TopRight
        CheckBoxDrawPrincipalPlanes.UseVisualStyleBackColor = True
        ' 
        ' FocalPlaneHalfWidthTextBox
        ' 
        FocalPlaneHalfWidthTextBox.Font = New Font("Segoe UI", 12F)
        FocalPlaneHalfWidthTextBox.Location = New Point(289, 180)
        FocalPlaneHalfWidthTextBox.Name = "FocalPlaneHalfWidthTextBox"
        FocalPlaneHalfWidthTextBox.Size = New Size(24, 34)
        FocalPlaneHalfWidthTextBox.TabIndex = 7
        FocalPlaneHalfWidthTextBox.Text = "5"
        ' 
        ' PrincipalPlaneHalfWidthTextBox
        ' 
        PrincipalPlaneHalfWidthTextBox.Font = New Font("Segoe UI", 12F)
        PrincipalPlaneHalfWidthTextBox.Location = New Point(326, 218)
        PrincipalPlaneHalfWidthTextBox.Name = "PrincipalPlaneHalfWidthTextBox"
        PrincipalPlaneHalfWidthTextBox.Size = New Size(24, 34)
        PrincipalPlaneHalfWidthTextBox.TabIndex = 8
        PrincipalPlaneHalfWidthTextBox.Text = "5"
        ' 
        ' ErrorProvider1
        ' 
        ErrorProvider1.ContainerControl = Me
        ' 
        ' Form1
        ' 
        AutoScaleDimensions = New SizeF(8F, 20F)
        AutoScaleMode = AutoScaleMode.Font
        ClientSize = New Size(536, 297)
        Controls.Add(PrincipalPlaneHalfWidthTextBox)
        Controls.Add(FocalPlaneHalfWidthTextBox)
        Controls.Add(CheckBoxDrawPrincipalPlanes)
        Controls.Add(CheckBoxDrawFocalPlanes)
        Controls.Add(CheckBoxDrawPrincipalPoints)
        Controls.Add(CheckBoxDrawFocalPoints)
        Controls.Add(LinkLabel1)
        Controls.Add(PictureBox1)
        Controls.Add(Button1)
        Name = "Form1"
        Text = "LightTools first order lens properties"
        CType(PictureBox1, ComponentModel.ISupportInitialize).EndInit()
        CType(ErrorProvider1, ComponentModel.ISupportInitialize).EndInit()
        ResumeLayout(False)
        PerformLayout()
    End Sub

    Friend WithEvents Button1 As Button
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents LinkLabel1 As LinkLabel
    Friend WithEvents CheckBoxDrawFocalPoints As CheckBox
    Friend WithEvents CheckBoxDrawPrincipalPoints As CheckBox
    Friend WithEvents CheckBoxDrawFocalPlanes As CheckBox
    Friend WithEvents CheckBoxDrawPrincipalPlanes As CheckBox
    Friend WithEvents FocalPlaneHalfWidthTextBox As TextBox
    Friend WithEvents PrincipalPlaneHalfWidthTextBox As TextBox
    Friend WithEvents ErrorProvider1 As ErrorProvider

End Class
