using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiocoTombola
{
    enum Livello
    {
        Facile,
        Medio,
        Difficile
    }

    class Tombola
    {
        // definisco un evento che lancerò quando il giocatore
        // avrà indovinato un numero
        public event EventHandler Indovinato;
        protected void OnIndovinato()
        {
            Indovinato?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler GiocoTerminato;
        protected void OnGiocoTerminato()
        {
            GiocoTerminato?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler Estratto;
        protected void OnEstratto()
        {
            Estratto?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler GiocoIniziato;
        protected void OnGiocoIniziato()
        {
            GiocoIniziato?.Invoke(this, EventArgs.Empty);
        }

        private const int N = 90;
        private bool[] _numeriGiocati;
        private int _numeriDaEstrarre;
        private GeneratoreCasuale.NRRandom _gc = new GeneratoreCasuale.NRRandom(1, N + 1);
        public int ContatoreEstratti { get; set; }
        public List<int> NumeriEstratti { get; } = new List<int>();
        public int ContatoreIndovinati { get; private set; }
        public List<int> NumeriIndovinati { get; } = new List<int>();

        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <exception cref="ArgumentException"/>
        public void Gioca(Livello livello, params int[] numeri)
        {
            int[] a = { 70, 40, 20 };
            _numeriDaEstrarre = a[(int)livello];

            // creo ogni volta un nuovo array così viene inizializzato
            // con tutti false
            _numeriGiocati = new bool[N + 1];
            ContatoreEstratti = 0;

            foreach (int n in numeri)
            {
                if (n < 1 || n > N)
                    throw new ArgumentOutOfRangeException($"I numeri devono essere compresi tra 1 e {N}");

                // se n è già stato giocato, nell'array _numeriGiocati
                // la cella di indice n contiene true

                if (_numeriGiocati[n])  // se ho già giocato il numero n
                    throw new ArgumentException("I numeri da giocare devono essere diversi");

                _numeriGiocati[n] = true;
            }

            OnGiocoIniziato();

            _gc.Reset();    // per ogni volta che si gioca
            NumeriEstratti.Clear();
            ContatoreIndovinati = 0;
            NumeriIndovinati.Clear();
        }

        private void AggiornaSituazione(int estratto)
        {
            if (_numeriGiocati[estratto])
            {
                // è stato indovinato un numero
                ContatoreIndovinati++;
                NumeriIndovinati.Add(estratto);

                // genero l'evento Indovinato
                // passaggio standard dei parametri
                // object this e EventArgs vuoto
                //if (Indovinato != null)
                //    Indovinato(this, EventArgs.Empty);

                // operatore ?. utilizza il membro solo se l'oggetto 
                // è diverso da null altrimenti non fa nulla
                // ?. è detto null conditional operatore
                // però è più conosciuto come Elvis operator
                // Indovinato?.Invoke(this, EventArgs.Empty);

                OnIndovinato();

                if (ContatoreIndovinati == 5)
                    OnGiocoTerminato();
            }
        }

        public int Estrai()
        {
            if (ContatoreEstratti >= _numeriDaEstrarre)
                throw new InvalidOperationException("Estrazioni esaurite");

            int estratto = _gc.Next();

            NumeriEstratti.Add(estratto);
            ContatoreEstratti++;

            OnEstratto();

            AggiornaSituazione(estratto);

            if (ContatoreEstratti == _numeriDaEstrarre)
                OnGiocoTerminato();

            return estratto;
        }
    }
}
