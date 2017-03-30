using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PPO
{
    public partial class Form1 : Form
    {

        public int xposmouse;
        public int yposmouse;
        public bool relleno = false;
        public Color ColorVar = Color.Black;
        public Graphics graphics;
        private int grosor = 1;
        private Herramientas miHerramienta;

        enum Herramientas
        {
            Lapiz,
            Circulo,
            Cuadrado,
            Borrador,
            Linea,
            Rectangulo,
        }


        public Form1()
        {
            InitializeComponent();
            graphics = pbLienzo.CreateGraphics();
        }

        private void pbLienzo_MouseMove(object sender, MouseEventArgs e)
        {
            slCoordenadas.Text = e.Location.ToString();
        }

        private void DibujarLinea(int xfinal, int yfinal, int inicialx, int inicialy, Color color, int grosor = 1)
        {
            int xi, yi, xf, yf;
            xi = yi = 0;
            xf = xfinal;
            yf = yfinal;
            int s1, s2, intercambio, x, y;
            float ei, ax, ay, temp;



            Pen pluma = new Pen(color, grosor);
            x = xi;
            y = yi;
            ax = Math.Abs(xf - xi);
            ay = Math.Abs(yf - yi);
            s1 = signo(xf - xi);
            s2 = signo(yf - yi);

            if (ay > ax)
            {
                temp = ax;
                ax = ay;
                ay = temp;
                intercambio = 1;
            }
            else
            {
                intercambio = 0;
            }

            ei = 2 * ay - ax;

            for (int m = 1; m < ax; m++)
            {
                int puntox, puntoy;
                puntox = inicialx + x;
                puntoy = inicialy - y;
                graphics.DrawRectangle(pluma, puntox, puntoy, 1, 1);
                if (ei >= 0)
                {
                    if (intercambio == 1)
                    {
                        x = x + s1;
                    }
                    else
                    {
                        y = y + s2;
                    }

                    ei = ei - (s2 * ax);
                }

                if (intercambio == 1)
                {
                    y = y + s2;
                }
                else
                {
                    x = x + s1;
                }

                ei = ei + 2 * ay;
            }
        }

        int signo(int num)
        {
            int resultado = 0;
            if (num < 0)
            {
                resultado = -1;
            }

            if (num > 0)
            {
                resultado = 1;
            }

            if (num == 0)
                return resultado;

            return resultado;
        }

        private void DibujarCirculo(int x, int y, int r, Color color, int grosor)
        {
            int x1 = 0;
            int y1 = r;
            int d = 3 - 2 * r;

            Pen pluma = new Pen(color, grosor);

            while (x1 <= y1)
            {
                graphics.DrawRectangle(pluma, x + x1, y + y1, 1, 1);
                graphics.DrawRectangle(pluma, x - x1, y + y1, 1, 1);
                graphics.DrawRectangle(pluma, x + x1, y - y1, 1, 1);
                graphics.DrawRectangle(pluma, x - x1, y - y1, 1, 1);
                graphics.DrawRectangle(pluma, x + y1, y + x1, 1, 1);
                graphics.DrawRectangle(pluma, x - y1, y + x1, 1, 1);
                graphics.DrawRectangle(pluma, x + y1, y - x1, 1, 1);
                graphics.DrawRectangle(pluma, x - y1, y - x1, 1, 1);
                if (d < 0)
                {
                    d += 4 * x1 + 6;
                }
                else
                {
                    d += 4 * (x1 - y1) + 10;
                    y1--;
                }
                x1++;
            }
        }

        private void btnLinea_Click(object sender, EventArgs e)
        {
            miHerramienta = Herramientas.Linea;
        }

        private void pbLienzo_MouseDown(object sender, MouseEventArgs e)
        {
            xposmouse = e.X;
            yposmouse = e.Y;
        }

        private void pbLienzo_MouseUp(object sender, MouseEventArgs e)
        {
            int a = e.X - xposmouse;
            int b = yposmouse - e.Y;

            switch (miHerramienta)
            {
                case (Herramientas.Linea):
                    DibujarLinea(a, b, xposmouse, yposmouse, ColorVar, grosor);
                    break;

                case (Herramientas.Circulo):
                    DibujarCirculo(xposmouse, yposmouse, a, ColorVar, grosor);
                    break;

                case (Herramientas.Rectangulo):
                    DibujarLinea(a, 0, xposmouse, yposmouse, ColorVar, grosor);
                    DibujarLinea(0, b, xposmouse, yposmouse, ColorVar, grosor);
                    DibujarLinea(-a, 0, e.X, e.Y, ColorVar, grosor);
                    DibujarLinea(0, -b, e.X, e.Y, ColorVar, grosor);
                    break;

                case (Herramientas.Cuadrado):
                    DibujarLinea(a, 0, xposmouse, yposmouse, ColorVar, grosor);
                    DibujarLinea(0, a, xposmouse, yposmouse, ColorVar, grosor);
                    DibujarLinea(a, 0, xposmouse, yposmouse - a, ColorVar, grosor);
                    DibujarLinea(0, a, xposmouse + a, yposmouse, ColorVar, grosor);
                    break;

            }


        }

        private void btnCirculo_Click(object sender, EventArgs e)
        {
            miHerramienta = Herramientas.Circulo;
        }

        private void btnLapiz_Click(object sender, EventArgs e)
        {
            miHerramienta = Herramientas.Lapiz;
        }

        private void btnBorrador_Click(object sender, EventArgs e)
        {
            miHerramienta = Herramientas.Borrador;
        }

        private void btnCuadrado_Click(object sender, EventArgs e)
        {
            miHerramienta = Herramientas.Cuadrado;
        }

        private void btnRectangulo_Click(object sender, EventArgs e)
        {
            miHerramienta = Herramientas.Rectangulo;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            pbPincelPrueba.Refresh();
            grosor = trackBar1.Value;
            lblGrosor.Text = trackBar1.Value.ToString();
            Graphics pincelprueba = pbPincelPrueba.CreateGraphics();
            Pen pincel = new Pen(ColorVar, grosor);
            pincelprueba.DrawRectangle(pincel, pbPincelPrueba.Width/2, pbPincelPrueba.Height/2, 1 ,1);
        }
    }
}
