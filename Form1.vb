Imports System.Data.SqlClient
Imports System.Runtime.CompilerServices
Imports System.Windows.Forms.DataVisualization.Charting


Public Class Form1

    Public ConStr As String = "Data Source=PC;Initial Catalog=HarcamaListe;Persist Security Info=True;User ID=sa;Password=1234;"
    Public connection As New SqlConnection(ConStr)
    Public ort As Integer
    Public toplam As Integer
    Public seciliKisiId As Integer
    Public seciliId As Integer
    Public kisi1harcama As Double
    Public kisi2harcama As Double
    Public HarcamaOrtalama As Double
    Public seciliAy As Integer = 0
    Public seciliAyToplam As Integer = 0



    'Public Sub DbCon()
    '    Try
    '        connection.Open()

    '    Catch ex As Exception
    '        MessageBox.Show("Bağlantı Hatası : & ex.Message")
    '    End Try
    '    connection.Close()
    'End Sub

    Public Function OrtalamaAl(ByVal sayi1 As Double, ByVal sayi2 As Double)
        Dim OrtalamaDeger As Double
        OrtalamaDeger = (sayi1 + sayi2) / 2
        Return OrtalamaDeger
    End Function

    Public Sub DataGridDoldur()

        Dim query As String = "select * from HarcamaListesi hc order by hc.HarcamaTarihi desc"
        Using connection As New SqlConnection(ConStr)
            Using command As New SqlCommand(query, connection)
                Try
                    connection.Open()
                    Dim adapter As New SqlDataAdapter(command)
                    Dim table As New DataTable()
                    adapter.Fill(table)
                    DataGridView1.DataSource = table
                    'DataGridView1.Rows(0).Height = 50   - ilk satırın yüksekliğini ayarlama
                Catch ex As Exception
                    MessageBox.Show("Veri Okuma Hatası : " & ex.Message)
                Finally
                    connection.Close()
                End Try
            End Using
        End Using
        KişiToplamAl()
    End Sub

    Public Sub KisiIdSec()

        'İki sayfayaki seçili olan radiobuttonların Id olarak alır'
        If rb1.Checked Or rb3.Checked = True Then
            seciliKisiId = 1
        ElseIf rb2.Checked Or rb4.Checked = True Then
            seciliKisiId = 2
        Else

        End If
    End Sub
    Public Sub AySec()
        'If ComboBox1.SelectedIndex = 0 Then
        '    seciliAy = 1
        'ElseIf ComboBox1.SelectedIndex = 1 Then
        '    seciliAy = 2
        'ElseIf ComboBox1.SelectedIndex = 2 Then
        '    seciliAy = 3
        'ElseIf ComboBox1.SelectedIndex = 3 Then
        '    seciliAy = 4
        'ElseIf ComboBox1.SelectedIndex = 4 Then
        '    seciliAy = 5
        'ElseIf ComboBox1.SelectedIndex = 5 Then
        '    seciliAy = 6
        'ElseIf ComboBox1.SelectedIndex = 6 Then
        '    seciliAy = 7
        'ElseIf ComboBox1.SelectedIndex = 7 Then
        '    seciliAy = 8
        'ElseIf ComboBox1.SelectedIndex = 8 Then
        '    seciliAy = 9
        'ElseIf ComboBox1.SelectedIndex = 9 Then
        '    seciliAy = 10
        'ElseIf ComboBox1.SelectedIndex = 10 Then
        '    seciliAy = 11
        'ElseIf ComboBox1.SelectedIndex = 11 Then
        '    seciliAy = 12
        'End If


        'OrElse, Visual Basic'te mantıksal operatörlerden biridir. OrElse, iki koşuldan en az birinin True olduğu
        'durumlarda True değerini döndürür

        If ComboBox1.SelectedIndex = 0 OrElse
           ComboBox1.SelectedIndex = 1 OrElse
           ComboBox1.SelectedIndex = 2 OrElse
           ComboBox1.SelectedIndex = 3 OrElse
           ComboBox1.SelectedIndex = 4 OrElse
           ComboBox1.SelectedIndex = 5 OrElse
           ComboBox1.SelectedIndex = 6 OrElse
           ComboBox1.SelectedIndex = 7 OrElse
           ComboBox1.SelectedIndex = 8 OrElse
           ComboBox1.SelectedIndex = 9 OrElse
           ComboBox1.SelectedIndex = 10 OrElse
           ComboBox1.SelectedIndex = 11 Then
            seciliAy = (ComboBox1.SelectedIndex) + 1
        End If
    End Sub
    Public Sub VeriEkle()
        KisiIdSec()
        If seciliKisiId = Nothing Then
            MessageBox.Show("Lütfen Kişi Seçimi Yapınız")
        Else
            Dim seciiTarih As DateTime = DateTimePicker1.Value
            Dim sqlFormattedDate As String = seciiTarih.ToString("yyyy-MM-dd")
            Using connection As New SqlConnection(ConStr)
                connection.Open()
                Dim sorgu As String = String.Format("Insert into HarcamaListesi values ({0},{1},'{2}','{3}')", seciliKisiId, Convert.ToDouble(TextBox1.Text), sqlFormattedDate, Convert.ToString(TextBox4.Text))
                Using Command As New SqlCommand(sorgu, connection)
                    Command.ExecuteNonQuery()
                End Using
                'MessageBox.Show("Tarih başarıyla eklendi.")
                connection.Close()
            End Using
        End If
        DataGridDoldur()
    End Sub
    Public Sub KişiToplamAl()
        connection.Open()
        Dim toplamDegerKisi1 As Double
        Dim toplamDegerKisi2 As Double
        Dim sorgu As String = "select hl.HarcamaMiktari,hl.KisiId from HarcamaListesi hl"
        Dim komut As New SqlCommand(sorgu, connection)
        Dim oku As SqlDataReader = komut.ExecuteReader()
        While oku.Read()
            If oku("KisiId") = 1 Then
                Dim deger1 As Double = oku("HarcamaMiktari")
                toplamDegerKisi1 = deger1 + toplamDegerKisi1
            Else
                Dim deger2 As Double = oku("HarcamaMiktari")
                toplamDegerKisi2 = deger2 + toplamDegerKisi2
            End If
        End While
        TextBox2.Text = toplamDegerKisi1.ToString()
        TextBox3.Text = toplamDegerKisi2.ToString()
        TextBox7.Text = OrtalamaAl(Convert.ToDouble(TextBox2.Text), Convert.ToDouble(TextBox3.Text))

        kisi1harcama = Convert.ToDouble(TextBox2.Text)
        kisi2harcama = Convert.ToDouble(TextBox3.Text)
        HarcamaOrtalama = Convert.ToDouble(TextBox7.Text)

        If kisi1harcama > kisi2harcama Then
            TextBox5.Text = "0"
            TextBox6.Text = (HarcamaOrtalama - kisi2harcama).ToString()
        ElseIf kisi2harcama > kisi1harcama Then
            TextBox6.Text = "0"
            TextBox5.Text = (HarcamaOrtalama - kisi1harcama).ToString()
        Else
            TextBox5.Text = "Eşit"
            TextBox6.Text = "Eşit"
        End If

        connection.Close()
    End Sub

    Public Sub Tab2KişiToplamAl(ByVal sorgu As String)
        connection.Open()
        KisiIdSec()
        Dim toplamDegerKisi1 As Double
        Dim toplamDegerKisi2 As Double
        Dim komut As New SqlCommand(sorgu, connection)
        Dim oku As SqlDataReader = komut.ExecuteReader()
        While oku.Read()
            If oku("KisiId") = 1 Then
                Dim deger1 As Double = oku("HarcamaMiktari")
                toplamDegerKisi1 = deger1 + toplamDegerKisi1
            Else
                Dim deger2 As Double = oku("HarcamaMiktari")
                toplamDegerKisi2 = deger2 + toplamDegerKisi2
            End If
        End While
        TextBox8.Text = toplamDegerKisi1.ToString()
        TextBox9.Text = toplamDegerKisi2.ToString()
        TextBox10.Text = (toplamDegerKisi1 + toplamDegerKisi2).ToString

        kisi1harcama = Convert.ToDouble(TextBox8.Text)
        kisi2harcama = Convert.ToDouble(TextBox9.Text)

        connection.Close()
    End Sub

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        DataGridDoldur()
        KişiToplamAl()


    End Sub

    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click

    End Sub

    Private Sub BtnEkle_Click(sender As Object, e As EventArgs) Handles BtnEkle.Click
        VeriEkle()
        KişiToplamAl()


    End Sub
    'Seçili Satırın Bilgilerini Alma
    Private Sub DataGridView1_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellContentClick
        Dim rowIndex As Integer = 0
        If e.RowIndex >= 0 Then
            rowIndex = e.RowIndex
        Else
            rowIndex = 0
        End If
        'Seçilen rowsun text lere yazırma
        TextBox1.Text = DataGridView1.Rows(rowIndex).Cells("HarcamaMiktari").Value.ToString()
        TextBox4.Text = DataGridView1.Rows(rowIndex).Cells("Aciklama").Value.ToString()
        DateTimePicker1.Text = DataGridView1.Rows(rowIndex).Cells("HarcamaTarihi").Value.ToString()
        seciliKisiId = DataGridView1.Rows(rowIndex).Cells("KisiId").Value
        seciliId = DataGridView1.Rows(rowIndex).Cells("Id").Value

        'Secili Radio Buttonu Alma'
        If seciliKisiId = 1 Then
            rb1.Checked = True
        ElseIf seciliKisiId = 2 Then
            rb2.Checked = True
        End If

    End Sub

    Private Sub btnSil_Click(sender As Object, e As EventArgs) Handles btnSil.Click
        KisiIdSec()
        If seciliKisiId = Nothing Then
            MessageBox.Show("Lütfen Kişi Seçimi Yapınız")
        Else
            Using connection As New SqlConnection(ConStr)
                connection.Open()
                Dim sorgu As String = String.Format("DELETE from HarcamaListesi where Id= '{0}'", seciliId)
                Using Command As New SqlCommand(sorgu, connection)
                    Command.ExecuteNonQuery()
                End Using
                'MessageBox.Show("Tarih başarıyla eklendi.")
                connection.Close()
            End Using
        End If
        DataGridDoldur()

    End Sub

    Public Sub VeriGetir()
        KisiIdSec()
        'AySec()

        Dim query As String = String.Format("Select * from HarcamaListesi hl where DATEPART(MONTH,HarcamaTarihi)='{0}' and hl.KisiId='{1}'", seciliAy, seciliKisiId)
        Using connection As New SqlConnection(ConStr)
            Using Command As New SqlCommand(query, connection)
                Try
                    connection.Open()
                    Dim adapter As New SqlDataAdapter(Command)
                    Dim table As New DataTable()
                    adapter.Fill(table)

                    DataGridView2.DataSource = table

                Catch ex As Exception
                    MessageBox.Show("Veri Okuma Hatası : " & ex.Message)
                    connection.Close()
                End Try
            End Using
        End Using
    End Sub

    Public Sub SorguylaVeriTopla(ByVal query As String)
        KisiIdSec()
        AySec()
        connection.Open()
        Dim toplamAyDegerKisi1 As Double = 0
        Dim toplamAyDegerKisi2 As Double = 0
        Dim sorgu As String = query
        Dim komut As New SqlCommand(sorgu, connection)
        Dim oku As SqlDataReader = komut.ExecuteReader()
        While oku.Read()
            If oku("KisiId") = 1 Then
                Dim deger1 As Double = oku("HarcamaMiktari")
                toplamAyDegerKisi1 = deger1 + toplamAyDegerKisi1
            Else
                Dim deger2 As Double = oku("HarcamaMiktari")
                toplamAyDegerKisi2 = deger2 + toplamAyDegerKisi2
            End If
        End While
        TextBox8.Text = toplamAyDegerKisi1.ToString()
        TextBox9.Text = toplamAyDegerKisi2.ToString()
        TextBox10.Text = OrtalamaAl(Convert.ToDouble(TextBox8.Text), Convert.ToDouble(TextBox9.Text))
        connection.Close()
    End Sub

    Private Sub btnVeriGetir_Click(sender As Object, e As EventArgs) Handles btnVeriGetir.Click
        VeriGetir()
        SorguylaVeriTopla(String.Format("Select * from HarcamaListesi hl where DATEPART(MONTH,HarcamaTarihi)='{0}' and hl.KisiId='{1}'", seciliAy, seciliId))
        Tab2KişiToplamAl(String.Format("Select * from HarcamaListesi hl where DATEPART(MONTH,HarcamaTarihi)='{0}' and hl.KisiId='{1}'", seciliAy, seciliKisiId))
        seciliAyToplamAl()
    End Sub


    Private Sub ComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ComboBox1.SelectedIndexChanged
        AySec()
    End Sub

    Public Sub seciliAyToplamAl()
        seciliAyToplam = 0
        AySec()
        connection.Open()
        Dim sorgu As String = String.Format("Select * from HarcamaListesi hl where DATEPART(MONTH,HarcamaTarihi)='{0}'", seciliAy)
        Dim komut = New SqlCommand(sorgu, connection)
        Dim oku As SqlDataReader = komut.ExecuteReader()
        While oku.Read()
            If seciliAy <> Nothing Then
                Dim deger1 As Double = oku("HarcamaMiktari")
                seciliAyToplam = deger1 + seciliAyToplam
            End If
        End While
        TextBox10.Text = seciliAyToplam.ToString()
        connection.Close()
    End Sub

    Public Sub GrafikCiz()
        connection.Open()
        AySec()
        Dim sorgu As String = String.Format("SELECT  DATEPART(MONTH, hl.HarcamaTarihi) Ay,SUM(hl.HarcamaMiktari) AS ToplamHarcama FROM HarcamaListesi hl WHERE DATEPART(MONTH, hl.HarcamaTarihi) IN (1,2,3,4,5,6,7,8,9,10,11,12) GROUP BY DATEPART(MONTH, hl.HarcamaTarihi)")
        Dim komut = New SqlDataAdapter(sorgu, connection)
        Dim table As New DataTable()
        komut.Fill(table)

        Dim satırSayısı As Integer = 0
        'Grafiğin pointlerini siler 
        Chart1.Series("Aylar").Points.Clear()

        ' tablo satırlarıyla' 

        'While satırSayısı < 12
        '    If table.Rows(satırSayısı).Item("ToplamHarcama") <> Nothing Then
        '        Chart1.Series("Aylar").Points.AddY(table.Rows(satırSayısı).Item("ToplamHarcama"))
        '        satırSayısı += 1
        '    End If
        'End While


        For i = 0 To (12 - table.Rows.Count)
            table.Rows.Add(table.Rows.Count + i + 1, "0")
        Next


        For i = 1 To 12
            For j = 1 To 12
                If table.Rows(j - 1).Item("Ay") = i Then
                    Chart1.Series("Aylar").Points.AddXY(i, table.Rows(j - 1).Item("ToplamHarcama"))
                    Exit For
                End If
            Next
        Next


        'Chart1.Series("Aylar").Points.AddY(table.Rows(10).Item("ToplamHarcama"))
        'Chart1.Series("Aylar").Points.AddY(table.Rows(11).Item("ToplamHarcama"))


        connection.Close()
    End Sub


    Private Sub tb1_SelectedIndexChanged(sender As Object, e As EventArgs)
        GrafikCiz()
    End Sub
End Class
