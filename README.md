# Sistema de Registro Civil

Olá! Este é um sistema desenvolvido para cartórios civis, que permite o registro de nascimentos, casamentos e óbitos de forma simples e eficiente.

## Como começar

### Pré-requisitos

Antes de começar, você vai precisar ter instalado:
- [Visual Studio 2022](https://visualstudio.microsoft.com/pt-br/vs/) (ou superior) - opcional
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL 15](https://www.postgresql.org/download/) (ou superior)
- [Npgsql](https://www.npgsql.org/) (já incluído no projeto)

### Configuração do Ambiente

1. **Clonar o Repositório**
   ```bash
   git clone https://github.com/henr1q/DeMaria
   cd DeMaria
   ```

2. **Configuração do Banco de Dados**
   - Instale o PostgreSQL se ainda não tiver
   - Execute o script de configuração do banco de dados:
     ```powershell
     .\setup-database.ps1
     ```
   - O script vai:
     - Verificar se o PostgreSQL está instalado
     - Criar o banco de dados `cartorio`
     - Configurar o arquivo `appsettings.json` com a string de conexão
     - Instalar as ferramentas do Entity Framework Core (se necessário)
     - Executar as migrações do banco de dados
   - Se precisar alterar as configurações padrão (usuário, senha, etc.), edite o arquivo `setup-database.ps1`

3. **Configuração Manual do Banco de Dados (opcional)**
   Se preferir configurar manualmente:
   - Crie um banco de dados chamado `cartorio`
   - Ajuste a string de conexão no arquivo `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=seu_servidor;Port=5432;Database=cartorio;User Id=seu_usuario;Password=sua_senha;"
     }
   }
   ```
   - Instale as ferramentas do Entity Framework Core:
     ```bash
     dotnet tool install --global dotnet-ef
     ```
   - Execute as migrações do banco de dados:
     ```bash
     dotnet ef database update
     ```

### Rodando o Projeto

#### Usando Visual Studio (Recomendado para Desenvolvimento)
1. Abra o projeto no Visual Studio:
   - Abra o arquivo `DeMaria.sln`
   - Aguarde o Visual Studio restaurar os pacotes NuGet

2. Pressione F5 ou clique em "Iniciar" para rodar o projeto

#### Usando Terminal e dotnet (Se quiser apenas rodar o projeto)
1. Restaure os pacotes NuGet:
   ```bash
   dotnet restore
   ```

2. Compile o projeto:
   ```bash
   dotnet build
   ```

3. Execute o projeto:
   ```bash
   dotnet run
   ```


## Tecnologias Usadas

- **Frontend**: Windows Forms (porque é prático e rápido!)
- **Backend**: .NET 9.0
- **Banco de Dados**: PostgreSQL (o mais robusto e confiável)
- **ORM**: Entity Framework Core (pra facilitar nossa vida com o banco)
- **Relatórios**: ReportViewer (pra gerar aqueles relatórios bonitões)

## Estrutura do Projeto

```
DeMaria/
├── Forms/              # Toda a interface do usuário
│   ├── RegistroNascimentoForm.cs
│   ├── RegistroCasamentoForm.cs
│   ├── RegistroObitoForm.cs
│   └── Relatorios/     # Formulários de relatórios
├── Models/             # Nossas classes de modelo
├── Data/               # Configuração do banco de dados
└── Reports/            # Templates dos relatórios
```

## Funcionalidades

### Registros
- **Nascimento**: Registre todos os detalhes do bebê e dos pais
- **Casamento**: Cadastre os noivos com todas as informações necessárias
- **Óbito**: Registre os dados do falecido e familiares

### Relatórios
- Gere relatórios por período
- Exporte para XML com um clique
- Visualize todos os registros de forma organizada

## Precisa de Ajuda?

Se tiver qualquer dúvida ou problema:
1. Verifique se todos os pré-requisitos estão instalados
2. Confira se a string de conexão está correta

