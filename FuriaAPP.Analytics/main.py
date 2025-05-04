from api_client import APIClient
from jogo_service import JogoService
from navegacao import Navegacao

class MainApp:
    def __init__(self):
        self.api_client = APIClient()
        self.navegacao = Navegacao()

    def menu_principal(self) -> int:
        opcoes = [
            "Realizar nova consulta",
            "Sair do sistema"
        ]
        self.navegacao.mostrar_menu("SISTEMA DE CONSULTA DE INTERESSES EM JOGOS", opcoes)
        return self.navegacao.obter_opcao(len(opcoes))

    def realizar_consulta(self, _=None):
        self.navegacao.limpar_console()
        
        jogos = self.api_client.get_jogos()
        if jogos:
            JogoService.listar_jogos(jogos)
        
        try:
            jogo_id = int(input("\nDigite o ID do jogo para localizar interessados: "))
            self.navegacao.limpar_console()
        except ValueError:
            input("ID inválido. Deve ser um número inteiro. Pressione Enter para continuar...")
            return
        
        usuarios = self.api_client.get_usuarios()
        if usuarios:
            resultado = JogoService.localizar_interesse(usuarios, jogo_id, jogos)
            
            if resultado['interessados']:
                print(f"\nUsuários interessados em '{resultado['nome_jogo']}':")
                print("Nome".ljust(20) + "E-mail")
                print("-" * 50)
                for usuario in resultado['interessados']:
                    print(f"{usuario['nome'].ljust(20)}{usuario['email']}")
            else:
                print(f"\nNenhum usuário encontrado com interesse em '{resultado['nome_jogo']}'")

    def executar(self):
        self.navegacao.executar_em_loop(self.menu_principal, self.realizar_consulta)

if __name__ == "__main__":
    app = MainApp()
    app.executar()