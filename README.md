<h1 align="center" >
   Blue Modas | Backend  
</h1>

<p align="center">
  <a href='#home'>Blue Modas </a>|
  <a href='#banco'>Banco de dados </a>|
  <a href='#rotas'>Rotas </a>|
  <a href='#tecnologies'>Tecnologias </a>|
  <a href='#layout'>Layout </a>|
  <a href="#como">Como usar </a>
</p>

## <p id='home'>🎲 Blue Modas | Backend </p>
Desenvolvimento da API para atender demandas do e-commerce Blue Modas.

## <p id='banco'> 🎲 Banco de dados </p>
1. Modelagem de banco
![image](https://user-images.githubusercontent.com/55838972/114646639-9fd6e980-9ca9-11eb-932a-18f62fd7d639.png)

2. Script para criação das tabelas.
```sql
CREATE DATABASE BD_BlueModas

USE BD_BlueModas

CREATE TABLE Produto (
	produtoId int primary key identity(1,1),
	nomeProduto varchar(100) not null,
	precoProduto decimal(18,2) not null,
	urlImage varchar(1000) null
)

CREATE TABLE Cliente (
	clienteId int primary key identity(1,1),
	nomeCliente varchar(100) not null,
	email varchar(100) not null,
	telefone varchar(100) not null
)

CREATE TABLE CestaCompra (
	cestaCompraId int primary key identity(1,1),
	clienteId int null,
	constraint fk_CestaCompra_Cliente foreign key (clienteId) references Cliente (clienteId) 
)

CREATE TABLE ItemCompra (
	itemCompraId int primary key identity(1,1),
	cestaCompraId int not null,
	produtoId int not null,
	quantidade int not null,
	valorUnitario decimal(18,2) not null,
	constraint fk_ItemCompra_CestaCompra foreign key (cestaCompraId) references CestaCompra (cestaCompraId),
	constraint fk_ItemCompra_Produto foreign key (produtoId) references Produto (produtoId) 
)

```

## <p id='rotas'> ⚙ Rotas da aplicação</p>
Produto
- **`GET /produto`**: Essa rota retorna uma listagem com todos os produtos cadastrados no banco.
```json
[
    {
        "produtoId": 1,
        "nomeProduto": "CAMISETA ESTAMPA GATO SENDO ABDUZIDO BRILHA NO ESCURO PRETO",
        "precoProduto": 49.90,
        "urlImage": "http://localhost:59034/Assets/Images/img01.webp"
    },
    {
        "produtoId": 2,
        "nomeProduto": "BLUSA INFANTIL TIE DYE LISA SIMPSON",
        "precoProduto": 49.90,
        "urlImage": "http://localhost:59034/Assets/Images/img02.webp"
    },
    {
        "produtoId": 3,
        "nomeProduto": "BLUSÃO EM MOLETOM COM BOLSO CANGURU ESTAMPA LOS ANGELES ROSA",
        "precoProduto": 139.90,
        "urlImage": "http://localhost:59034/Assets/Images/img03.webp"
    }
]
```

- **`GET /produto/2`**: Essa rota retorna um produto pelo id.
```json
{
    "produtoId": 2,
    "nomeProduto": "BLUSA INFANTIL TIE DYE LISA SIMPSON",
    "precoProduto": 49.90,
    "urlImage": "http://localhost:59034/Assets/Images/img02.webp"
}
```

- **`POST /produto`**: Essa rota irá cadastrar um novo produto, sendo obrigatório informar no corpo da requisição a seguinte estrutura.
![image](https://user-images.githubusercontent.com/55838972/114754015-1a454f00-9d26-11eb-97ed-2b2d53ffdbde.png)

**`PUT /produto/6`**: Essa rota irá atualizar o produto já existente pelo id, sendo obrigatório informar nome e preço.
```json
{
	"nomeProduto": "BLUSA MANGA CURTA TIE DYE COM ESTAMPA URSINHO POOH E AMIGOS MULTICORES",
	"precoProduto": 29.90
}
```

- **`DELETE /produto/6`**: Essa rota irá deletar o produto já existente, basta informar somente o id do produto.

Cesta de compra
**`POST /CestaCompra/Adicionar`**: Essa rota irá adicionar um produto para a cesta, onde o id da cesta não é obrigatório para esse cadastro.
```json
{
	"idProduto": 3,
	"idCompra": null
}
```

**`PUT /CestaCompra/incrementar/120`**: Essa rota irá adicionar a quantidade do item na cesta, sendo necessário informar o id do item na cesta.

**`PUT /CestaCompra/decrementar/120`**: Essa rota irá diminuir a quantidade do item na cesta, sendo necessário informar o id do item na cesta.

**`PUT /CestaCompra/finalizar-compra/26`**: Essa rota irá finalizar a compra e vincular a cesta para o cliente.
```json
{
	"NomeCliente":"Diego Luques",
	"Email": "luquesdiego@outlook.com",
	"Telefone": "(65) 98888-8888"
}
```

- **`DELETE /CestaCompra/deletar-item/119`**: Essa rota irá deletar o produto da cesta, basta informar o id do produto na cesta.

- **`GET /CestaCompra/resumo/20`**: Essa rota retorna o resumo da cesta para compras já finalizadas, contendo as informações.
```json
{
    "cestaCompraId": 20,
    "cliente": {
        "clienteId": 4,
        "nomeCliente": "Diogo",
        "email": "diogo@teste.com",
        "telefone": "(65) 98136-4360"
    },
    "itens": [
        {
            "itemCompraId": 71,
            "cestaCompraId": 20,
            "cestaCompra": {
                "cestaCompraId": 20,
                "clienteId": 4,
                "cliente": {
                    "clienteId": 4,
                    "nomeCliente": "Diogo",
                    "email": "diogo@teste.com",
                    "telefone": "(65) 98136-4360",
                    "cestasCompra": []
                }
            },
            "produtoId": 2,
            "produto": {
                "produtoId": 2,
                "nomeProduto": "BLUSA INFANTIL TIE DYE LISA SIMPSON",
                "precoProduto": 49.90,
                "urlImage": "http://localhost:59034/Assets/Images/img02.webp",
                "itensCompra": []
            },
            "quantidade": 1,
            "valorUnitario": 49.90
        },
        {
            "itemCompraId": 72,
            "cestaCompraId": 20,
            "cestaCompra": {
                "cestaCompraId": 20,
                "clienteId": 4,
                "cliente": {
                    "clienteId": 4,
                    "nomeCliente": "Diogo",
                    "email": "diogo@teste.com",
                    "telefone": "(65) 98136-4360",
                    "cestasCompra": []
                }
            },
            "produtoId": 4,
            "produto": {
                "produtoId": 4,
                "nomeProduto": "CUECA SAMBA CANÇÃO BART SIMPSON AZUL",
                "precoProduto": 39.90,
                "urlImage": "http://localhost:59034/Assets/Images/img04.webp",
                "itensCompra": []
            },
            "quantidade": 1,
            "valorUnitario": 39.90
        }
    ],
    "valorTotal": 89.80
}
```

- **`GET /CestaCompra/resumo-cesta/21`**: Essa rota retorna o resumo da cesta, contendo as informações.
```json
{
    "cestaCompraId": 21,
    "itens": [
        {
            "itemCompraId": 74,
            "cestaCompraId": 21,
            "cestaCompra": {
                "cestaCompraId": 21,
                "clienteId": 5
            },
            "produtoId": 2,
            "produto": {
                "produtoId": 2,
                "nomeProduto": "BLUSA INFANTIL TIE DYE LISA SIMPSON",
                "precoProduto": 49.90,
                "urlImage": "http://localhost:59034/Assets/Images/img02.webp",
                "itensCompra": []
            },
            "quantidade": 1,
            "valorUnitario": 49.90
        }
    ],
    "valorTotal": 49.90
}
```

## <p id='tecnologies'>💻 Tecnologias </p>
Este projeto foi desenvolvido com as seguintes tecnologias:

-  [.Net Core 3.1](https://dotnet.microsoft.com/download)
-  [Asp .Net Core 3.0.0](https://dotnet.microsoft.com/download)
-  [Entity Framework Core 5.0.5](https://dotnet.microsoft.com/download)

## <p id='como'>💻 Como usar </p>
Para clonar e executar este aplicativo, na linha de comando:

```bash
# Clone este repositório
$ git clone https://github.com/diegoluques/blue-modas-backend/

# Vá para o repositório
$ cd blue-modas-backend

# Abre o arquivo BlueModas.sln com o Visual Studio
$ Build a solução e executa
```

