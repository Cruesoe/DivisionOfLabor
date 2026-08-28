# Regenerates About/preview.png for Division of Labor.
# Run with no arguments to reproduce the shipped image:
#     powershell -ExecutionPolicy Bypass -File .\Branding\compose-preview.ps1
#
# The tick boxes are lifted from preview-source.png as a transparent sprite
# (luminance becomes alpha), so they stay the original artwork at any size.
param(
  [string]$Src    = "$PSScriptRoot\preview-source.png",
  [string]$Out    = "$PSScriptRoot\..\About\preview.png",
  [double]$Cap    = 60,      # title cap height in px
  [double]$BoxW   = 720,     # rendered width of the tick-box row
  [double]$TitleY = 278,     # y centre of the title caps
  [double]$BoxY   = 428,     # y centre of the box row
  [double]$Track  = 0.055    # letter-spacing as a fraction of font size
)
Add-Type -AssemblyName System.Drawing
$W = 1280; $H = 720

# ---- lift the tick-box row off the original as a white sprite with alpha ----
$orig = New-Object System.Drawing.Bitmap($Src)
$cx0 = 210; $cy0 = 314; $cw = 862; $ch = 128        # box row bbox + padding
$sprite = New-Object System.Drawing.Bitmap($cw, $ch, [System.Drawing.Imaging.PixelFormat]::Format32bppArgb)
for ($y = 0; $y -lt $ch; $y++) {
  for ($x = 0; $x -lt $cw; $x++) {
    $c = $orig.GetPixel($cx0 + $x, $cy0 + $y)
    $lum = 0.299*$c.R + 0.587*$c.G + 0.114*$c.B
    $a = [int][math]::Round((([math]::Max($lum - 32, 0)) / (255 - 32)) * 255)
    $sprite.SetPixel($x, $y, [System.Drawing.Color]::FromArgb([math]::Min($a,255), 248, 248, 246))
  }
}
$orig.Dispose()

# ---- canvas: near-black with a soft central glow, like the original ----
$bmp = New-Object System.Drawing.Bitmap($W, $H)
$g = [System.Drawing.Graphics]::FromImage($bmp)
$g.SmoothingMode = [System.Drawing.Drawing2D.SmoothingMode]::AntiAlias
$g.InterpolationMode = [System.Drawing.Drawing2D.InterpolationMode]::HighQualityBicubic
$g.PixelOffsetMode = [System.Drawing.Drawing2D.PixelOffsetMode]::HighQuality
$g.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::AntiAlias
$g.Clear([System.Drawing.Color]::FromArgb(3,3,3))
$gp = New-Object System.Drawing.Drawing2D.GraphicsPath
$gp.AddEllipse(-240, -300, ($W + 480), ($H + 600))
$pb = New-Object System.Drawing.Drawing2D.PathGradientBrush($gp)
$pb.CenterPoint    = New-Object System.Drawing.PointF(640, 360)
$pb.CenterColor    = [System.Drawing.Color]::FromArgb(30,30,30)
$pb.SurroundColors = @([System.Drawing.Color]::FromArgb(3,3,3))
$g.FillPath($pb, $gp)

# ---- title ----
$fmt = [System.Drawing.StringFormat]::GenericTypographic
$text = "DIVISION OF LABOR"
$probe = New-Object System.Drawing.Bitmap(400, 300)
$pg = [System.Drawing.Graphics]::FromImage($probe)
$pg.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::AntiAlias
$size = $Cap
for ($k = 0; $k -lt 12; $k++) {
  $f = New-Object System.Drawing.Font("Arial", $size, [System.Drawing.FontStyle]::Bold, [System.Drawing.GraphicsUnit]::Pixel)
  $pg.Clear([System.Drawing.Color]::Black); $pg.DrawString("H", $f, [System.Drawing.Brushes]::White, 20, 40)
  $mn=9999;$mx=-1
  for ($y=0;$y -lt 300;$y++){for($x=0;$x -lt 200;$x++){if($probe.GetPixel($x,$y).R -gt 128){if($y -lt $mn){$mn=$y};if($y -gt $mx){$mx=$y}}}}
  $got = $mx-$mn+1
  if ([math]::Abs($got - $Cap) -le 1) { break }
  $size = $size * ($Cap / [math]::Max($got,1)); $f.Dispose()
}
$font = New-Object System.Drawing.Font("Arial", $size, [System.Drawing.FontStyle]::Bold, [System.Drawing.GraphicsUnit]::Pixel)
$tr = $size * $Track
$adv = @()
foreach ($chx in $text.ToCharArray()) {
  if ($chx -eq [char]32) { $adv += $size * 0.34 }
  else { $adv += $pg.MeasureString([string]$chx, $font, [System.Drawing.PointF]::Empty, $fmt).Width }
}
$total = ($adv | Measure-Object -Sum).Sum + $tr * ($text.Length - 1)

# find true cap box so the line sits exactly on TitleY
$p2 = New-Object System.Drawing.Bitmap($W, $H)
$g2 = [System.Drawing.Graphics]::FromImage($p2)
$g2.TextRenderingHint = [System.Drawing.Text.TextRenderingHint]::AntiAlias
$px = ($W - $total)/2.0
for ($i=0; $i -lt $text.Length; $i++) { $g2.DrawString([string]$text[$i], $font, [System.Drawing.Brushes]::White, (New-Object System.Drawing.PointF($px, 200.0)), $fmt); $px += $adv[$i] + $tr }
$g2.Dispose()
$mn=99999;$mx=-1
for ($y=0;$y -lt $H;$y++){for($x=0;$x -lt $W;$x+=3){if($p2.GetPixel($x,$y).R -gt 170){if($y -lt $mn){$mn=$y};if($y -gt $mx){$mx=$y}}}}
$p2.Dispose(); $pg.Dispose(); $probe.Dispose()
$yTop = 200.0 + ($TitleY - (($mn+$mx)/2.0))

$brush = New-Object System.Drawing.SolidBrush([System.Drawing.Color]::FromArgb(248,248,246))
$x = ($W - $total)/2.0
for ($i=0; $i -lt $text.Length; $i++) { $g.DrawString([string]$text[$i], $font, $brush, (New-Object System.Drawing.PointF($x, $yTop)), $fmt); $x += $adv[$i] + $tr }

# ---- tick boxes, scaled ----
$sc = $BoxW / 844.0                      # 844 = true ink width inside the sprite
$dw = $cw * $sc; $dh = $ch * $sc
$g.DrawImage($sprite, (New-Object System.Drawing.RectangleF((640 - $dw/2), ($BoxY - $dh/2), $dw, $dh)))
$sprite.Dispose()
$g.Dispose()

# ---- 8-bit greyscale out ----
$rnd = New-Object System.Random(20260828)
$o = New-Object System.Drawing.Bitmap($W, $H, [System.Drawing.Imaging.PixelFormat]::Format8bppIndexed)
$pal = $o.Palette; for ($i=0;$i -lt 256;$i++){ $pal.Entries[$i] = [System.Drawing.Color]::FromArgb($i,$i,$i) }; $o.Palette = $pal
$bd = $o.LockBits((New-Object System.Drawing.Rectangle(0,0,$W,$H)), [System.Drawing.Imaging.ImageLockMode]::WriteOnly, [System.Drawing.Imaging.PixelFormat]::Format8bppIndexed)
$row = New-Object byte[] $bd.Stride
for ($y=0;$y -lt $H;$y++){
  for ($x=0;$x -lt $W;$x++){ $c=$bmp.GetPixel($x,$y); $v=0.299*$c.R+0.587*$c.G+0.114*$c.B + ($rnd.NextDouble()-0.5)*1.6; $row[$x]=[byte][int][math]::Max(0,[math]::Min(255,[math]::Round($v))) }
  [System.Runtime.InteropServices.Marshal]::Copy($row,0,[IntPtr]($bd.Scan0.ToInt64()+($y*$bd.Stride)),$bd.Stride)
}
$o.UnlockBits($bd); $o.Save($Out, [System.Drawing.Imaging.ImageFormat]::Png); $o.Dispose(); $bmp.Dispose()
Write-Output ("{0}  cap={1} titleW={2:N0} boxW={3} boxH={4:N0} ratio={5:N2}  {6:N0} bytes" -f (Split-Path $Out -Leaf), $Cap, $total, $BoxW, (112*$sc), ((112*$sc)/$Cap), (Get-Item $Out).Length)
