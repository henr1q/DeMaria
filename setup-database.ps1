# Script para configuração do banco de dados PostgreSQL
# Este script cria o banco de dados e configura as permissões necessárias

# Configurações
$dbName = "cartorio"
$dbUser = "postgres"  # Altere para o usuário desejado
$dbPassword = "postgres"  # Altere para a senha desejada
$dbHost = "localhost"
$dbPort = "5432"

# Verifica se o PostgreSQL está instalado
try {
    $psqlVersion = & psql --version
    Write-Host "PostgreSQL encontrado: $psqlVersion"
} catch {
    Write-Host "Erro: PostgreSQL não encontrado. Por favor, instale o PostgreSQL antes de continuar."
    exit 1
}

# Cria o banco de dados
Write-Host "Criando banco de dados '$dbName'..."
$createDbCommand = "CREATE DATABASE $dbName;"
& psql -U $dbUser -h $dbHost -p $dbPort -c $createDbCommand

if ($LASTEXITCODE -eq 0) {
    Write-Host "Banco de dados criado com sucesso!"
} else {
    Write-Host "Erro ao criar banco de dados. Verifique se o usuário tem permissões suficientes."
    exit 1
}

# Atualiza o arquivo appsettings.json com a string de conexão
$connectionString = "Server=$dbHost;Port=$dbPort;Database=$dbName;User Id=$dbUser;Password=$dbPassword;"
$appSettingsPath = "appsettings.json"

if (Test-Path $appSettingsPath) {
    $jsonContent = Get-Content $appSettingsPath -Raw | ConvertFrom-Json
    $jsonContent.ConnectionStrings.DefaultConnection = $connectionString
    $jsonContent | ConvertTo-Json | Set-Content $appSettingsPath
    Write-Host "Arquivo appsettings.json atualizado com sucesso!"
} else {
    Write-Host "Arquivo appsettings.json não encontrado. Criando novo arquivo..."
    $jsonContent = @{
        "ConnectionStrings" = @{
            "DefaultConnection" = $connectionString
        }
    }
    $jsonContent | ConvertTo-Json | Set-Content $appSettingsPath
}

# Instala as ferramentas do Entity Framework Core se necessário
Write-Host "Verificando instalação das ferramentas do Entity Framework Core..."
try {
    $efVersion = & dotnet ef --version
    Write-Host "EF Core Tools encontrado: $efVersion"
} catch {
    Write-Host "Instalando as ferramentas do Entity Framework Core..."
    & dotnet tool install --global dotnet-ef
}

# Executa as migrações do banco de dados
Write-Host "Executando migrações do banco de dados..."
& dotnet ef database update

if ($LASTEXITCODE -eq 0) {
    Write-Host "Migrações executadas com sucesso!"
} else {
    Write-Host "Erro ao executar migrações. Verifique se o projeto está compilado corretamente."
    exit 1
}

Write-Host "Configuração do banco de dados concluída com sucesso!"
Write-Host "Agora você pode executar a aplicação." 