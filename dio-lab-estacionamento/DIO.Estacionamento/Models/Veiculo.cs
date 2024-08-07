using DIO.Estacionamento.Enums;

namespace DIO.Estacionamento.Models
{
    public class Veiculo
    {
        public string Placa { get; private set; }
        public MarcaVeiculo Marca { get; private set; }
        public TipoVeiculo Tipo { get; private set; }
        public ModeloVeiculo Modelo { get; private set; }
        public CorVeiculo Cor { get; private set; }
        public DateTime HoraEntrada { get; private set; }

        public Veiculo(string placa, MarcaVeiculo marca, TipoVeiculo tipo, ModeloVeiculo modelo, CorVeiculo cor)
        {
            Placa = placa;
            Marca = marca;
            Tipo = tipo;
            Modelo = modelo;
            Cor = cor;
            HoraEntrada = DateTime.Now;
        }

        public override string ToString()
        {
            return $"Placa: {Placa}\n" + 
                   $"Tipo: {Tipo}\n" +
                   $"Marca: {Marca}\n" +
                   $"Modelo: {Modelo}\n" + 
                   $"Cor: {Cor}\n";
        }
    }
}
