# FuriaAPP

O **FuriaAPP** é um projeto que centraliza conteúdos e interesses dos usuários relacionados à organização **FURIA**, uma das maiores organizações de eSports do Brasil. A aplicação permite registrar preferências, visualizar conteúdos interativos e realizar análises sobre os interesses dos usuários com base em jogos e histórico de interação.

O projeto é desenvolvido com **Blazor** no front-end, **.NET** no back-end e inclui integração com **Python** para análise de dados, consumindo os dados da API central.

## Demonstração

Assista ao vídeo abaixo para ver uma demonstração do FuriaAPP em funcionamento:

[![Demonstração do FuriaAPP](https://img.youtube.com/vi/sREV6qczRZk/0.jpg)](https://www.youtube.com/watch?v=sREV6qczRZk&ab_channel=GuilhermeAugusto)

## Funcionalidades

- **Listagem de conteúdos sobre a FURIA** com base nos interesses dos usuários.
- **Cadastro de interesses** relacionados a jogos e categorias.
- **API RESTful** para armazenar e gerenciar os dados.
- **Análise de dados com Python** consumindo a API para identificar padrões e gerar insights.
- **Autenticação JWT** para controle de acesso.

## Estrutura do Projeto

- **FuriaAPP.API**: API desenvolvida em .NET para gerenciamento de usuários, conteúdos e interesses.
- **FuriaAPP.Client**: Interface web construída com Blazor.
- **FuriaAPP.Shared**: Contém as classes e DTOs compartilhadas entre os projetos.
- **FuriaAPP.Analysis** : Scripts em Python que consomem a API para realizar análise dos dados de interesse dos usuários.

## Tecnologias Utilizadas

- **Blazor** (Web front-end)
- **.NET** (Back-end e API)
- **C#** e **SQL** (Lógica e banco de dados)
- **Python** (Módulo de análise de dados)
- **JWT Authentication** (Autenticação)

## Como Rodar o Projeto

1. Clone o repositório:
   ```bash
   git clone https://github.com/GuiAugus/FuriaAPP.git
