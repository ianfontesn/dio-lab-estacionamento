using DIO.Estacionamento.Enums;
using DIO.Estacionamento.Models;
using System.Globalization;

Estacionamento estacionamento;
bool encerrar = false;

IniciarAplicacao();

//LoopPrincipal 
void IniciarAplicacao()
{
    Console.WriteLine("Seja bem vindo ao Estacionamento do Fontes. \n" +
    "Em seu primeiro acesso, é necessário preencher os valores de seu empreendimento.\n\n" +
    "Preencha conforme solicitado.");

    PreencherPrecosIniciais();

    while (!encerrar)
    {
        MostrarMenu();
        ReceberOpcaoEscolhida();
    }

    Console.WriteLine("ATÉ A PRÓXIMA!");
}

//Preencher preços nos dicionarios de tipos (moto, carro, onibus, etc) para criar uma instancia do estacionamento.
void PreencherPrecosIniciais()
{
    Dictionary<TipoVeiculo, decimal> precosIniciais = [];
    Dictionary<TipoVeiculo, decimal> precosHora = [];
    bool isValid;

    foreach (var tipo in Enum.GetValues(typeof(TipoVeiculo)))
    {
        isValid = false;

        while (!isValid)
        {
            Console.WriteLine($"\nTipo de veículo: {tipo}");

            Console.Write("Preço inicial: ");
            string? inputPrecoInicial = Console.ReadLine();

            Console.Write("Preço por hora: ");
            string? inputPrecoHora = Console.ReadLine();


            if (decimal.TryParse(inputPrecoInicial, CultureInfo.InvariantCulture, out decimal precoInicial)
                && decimal.TryParse(inputPrecoHora, CultureInfo.InvariantCulture, out decimal precoHora))
            {
                precosIniciais.Add((TipoVeiculo)tipo, precoInicial);
                precosHora.Add((TipoVeiculo)tipo, precoHora);
                isValid = true;
            }
            else
            {
                Console.WriteLine("\nValor inválido. Insira novamente\n ");
            }
        }
    }

    estacionamento = new Estacionamento(precosIniciais, precosHora);
}

//Apresenta o menu de opções.
void MostrarMenu()
{
    Console.WriteLine(
        "===== ESTACIONAMENTO ===== \n\n" +
        "Digite o numero da opção desejada:\n" +
        "1 - Cadastrar veículo\n" +
        "2 - Remover veículo\n" +
        "3 - Listar veículos\n" +
        "4 - Encerrar");
}

//Switch de opções solicitadas ao usuário, executa as rotinas necessárioas de acordo com a opção
void ReceberOpcaoEscolhida()
{

    switch (Console.ReadLine())
    {
        case "1":
            RotinaAdicaoVeiculo();
            break;

        case "2":
            RotinaRemocaoVeiculo();
            break;

        case "3":
            estacionamento.ListarVeiculos();
            break;

        case "4":
            encerrar = true;
            break;

        default:
            Console.WriteLine("Opção inválida.");
            break;

    }
}

//Cria um novo veículo com base nos dados de entrada do usuário
Veiculo CriarNovoVeiculo()
{
    Veiculo? veiculo = null;
    bool encerrar = false;

    while (!encerrar)
    {
        Console.WriteLine("Placa: (xxx1234 ou xxx1x23) ou '0' para cancelar");
        string? valor = Console.ReadLine();

        if (valor == "0")
        {
            encerrar = true;
            break;
        }
        else
        {
            if (!estacionamento.VerificarPlacaValida(valor))
            {
                Console.WriteLine("Placa inválida.");
            }
            else
            {
                TipoVeiculo tipo = ObterValorEnum<TipoVeiculo>();
                ModeloVeiculo modelo = ObterValorEnum<ModeloVeiculo>();
                MarcaVeiculo marca = ObterValorEnum<MarcaVeiculo>();
                CorVeiculo cor = ObterValorEnum<CorVeiculo>();

                veiculo = new Veiculo(valor, marca, tipo, modelo, cor);
                encerrar = true;
            }
        }
    }

    return veiculo;
}

//Recebe um tipo Enum para criação do veículo, exibindo suas opções.
TEnum ObterValorEnum<TEnum>() where TEnum : Enum
{
    string tipos = "";
    bool valido = false;
    TEnum? tipoSelecionado = default;

    foreach (var tipo in Enum.GetValues(typeof(TEnum)))
    {
        tipos += $"{(int)tipo} - {tipo}\n";
    }

    Console.WriteLine($"\nDigite a opção desejada: \n{tipos}");

    while (!valido)
    {
        Console.Write("Opção: ");
        if (int.TryParse(Console.ReadLine(), out int opcao) && Enum.IsDefined(typeof(TEnum), opcao))
        {
            valido = true;
            tipoSelecionado = (TEnum)Enum.ToObject(typeof(TEnum), opcao);
        }
        else
        {
            Console.WriteLine("Opção inválida. Digite novamente.");
        }
    }

    return tipoSelecionado!;
}

//Rotina para remoção de veículos, exibe as mensagens e chama o estacionamento para realizar os calculos e remoção.
void RotinaRemocaoVeiculo()
{
    Console.Write("Digite a placa: ");
    string? placa = Console.ReadLine();
    Veiculo? veiculo = estacionamento.ObterVeiculo(placa);

    if (veiculo is not null)
    {
        Console.WriteLine($"Valor final: R${estacionamento.CalcularPrecoSaidaVeiculo(veiculo):F2}");
        Console.WriteLine($"Realizar pagamento? y/n");

        string? opcao = Console.ReadLine();

        if (opcao is not null)
        {
            opcao = opcao.ToLower();
        }

        switch (opcao)
        {
            case "y":
                estacionamento.RemoverVeiculo(veiculo.Placa);
                Console.WriteLine("Obrigado, volte sempre.");
                break;
            case "n":
                Console.WriteLine("Nenhuma ação necessária.");
                break;
            default:
                Console.WriteLine("Opção inválida");
                break;
        }

    }
    else
    {
        Console.WriteLine($"A placa {placa} não foi encontrada.");
    }

}

//Chama a função para criar o veículo e passa-o para o estacionamento para adiciona-lo a lista.
void RotinaAdicaoVeiculo()
{
    Veiculo? veiculo = CriarNovoVeiculo();

    if (veiculo is not null)
    {
        estacionamento.AdicionarVeiculo(veiculo);
    }

}