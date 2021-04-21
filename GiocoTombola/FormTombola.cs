using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GiocoTombola
{
    public partial class FormTombola : Form
    {
        Tombola _tombola = new Tombola();

        public FormTombola()
        {
            InitializeComponent();

            // Indovinato è l'evento che si potrà verificare 
            // associato all'oggetto _tombola

            // _tombola_indovinato è il gestore dell'evento Indovinato
            // che è un metodo privato della classe FormTombola

            // la riga seguente imposta un'associazione tra l'evento e
            // il suo gestore

            // in particolare afferma di aver fatto una iscrizione
            // all'evento Indovinato
            _tombola.Indovinato += _tombola_Indovinato;
            _tombola.GiocoTerminato += _tombola_GiocoTerminato;
            _tombola.Estratto += _tombola_Estratto;
            _tombola.GiocoIniziato += _tombola_GiocoIniziato;
        }

        private void _tombola_GiocoIniziato(object sender, EventArgs e)
        {
            listBoxEstratti.DataSource = _tombola.NumeriEstratti.ToList();
            buttonEstrai.Enabled = true;
            labelNumero.Text = "";
            labelContatoreEstratti.Text = _tombola.ContatoreEstratti.ToString();
            labelMessaggio.Text = "Buona fortuna";
        }

        private void _tombola_Estratto(object sender, EventArgs e)
        {
            listBoxEstratti.DataSource = _tombola.NumeriEstratti.ToList();
            labelContatoreEstratti.Text = _tombola.ContatoreEstratti.ToString();
        }

        private void _tombola_GiocoTerminato(object sender, EventArgs e)
        {
            buttonEstrai.Enabled = false;
        }

        private void _tombola_Indovinato(object sender, EventArgs e)
        {
            AggiornaSituazione();
        }

        private void buttonGioca_Click(object sender, EventArgs e)
        {
            Livello livello = Livello.Facile;

            if (radioButtonMedio.Checked)
                livello = Livello.Medio;
            else if (radioButtonDifficile.Checked)
                livello = Livello.Difficile;

            try
            {
                _tombola.Gioca(livello, (int)numericUpDown1.Value,
                    (int)numericUpDown2.Value, (int)numericUpDown3.Value,
                    (int)numericUpDown4.Value, (int)numericUpDown5.Value);
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "Tombola", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
        }

        private void FormTombola_Load(object sender, EventArgs e)
        {
            radioButtonFacile.Checked = true;
            buttonEstrai.Enabled = false;
        }

        private void buttonEstrai_Click(object sender, EventArgs e)
        {
            labelNumero.Text = _tombola.Estrai().ToString();
        }

        private void AggiornaSituazione()
        {
            switch (_tombola.ContatoreIndovinati)
            {
                case 2:
                    labelMessaggio.Text = "Hai fatto ambo: ";
                    break;
                case 3:
                    labelMessaggio.Text = "Hai fatto terno: ";
                    break;
                case 4:
                    labelMessaggio.Text = "Hai fatto quaterna: ";
                    break;
                case 5:
                    labelMessaggio.Text = "Hai fatto cinquina: ";
                    break;
                default:
                    labelMessaggio.Text = "Numeri Indovinati: ";
                    break;
            }

            foreach (int n in _tombola.NumeriIndovinati)
                labelMessaggio.Text += n.ToString() + " ";
        }
    }
}
