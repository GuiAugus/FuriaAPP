import os
import platform
from typing import List, Callable

class Navegacao:
    @staticmethod
    def limpar_console():
        sistema = platform.system().lower()
        os.system('cls' if sistema == 'windows' else 'clear')

    @staticmethod
    def mostrar_menu(titulo: str, opcoes: List[str]) -> None:
        print("\n" + "=" * 50)
        print(titulo.center(50))
        print("=" * 50)
        for i, opcao in enumerate(opcoes, 1):
            print(f"{i}. {opcao}")
        print("=" * 50)

    @staticmethod
    def obter_opcao(quantidade_opcoes: int) -> int:
        while True:
            try:
                opcao = int(input("Digite sua opção: "))
                if 1 <= opcao <= quantidade_opcoes:
                    return opcao
                print(f"Opção inválida. Digite um número entre 1 e {quantidade_opcoes}.")
            except ValueError:
                print("Entrada inválida. Digite um número.")

    @staticmethod
    def executar_em_loop(menu_func: Callable, acao_func: Callable) -> None:
        while True:
            Navegacao.limpar_console()
            opcao = menu_func()
            
            if opcao == 2: 
                Navegacao.limpar_console()
                print("\nSistema encerrado. Até logo!")
                break
                
            acao_func(opcao)
            input("\nPressione Enter para continuar...")