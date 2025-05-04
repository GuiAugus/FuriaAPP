from typing import List, Dict

class JogoService:
    @staticmethod
    def localizar_interesse(usuarios: List[Dict], jogo_id: int, todos_jogos: List[Dict] = None) -> Dict:
        interessados = []
        nome_jogo = JogoService.obter_nome_jogo(todos_jogos, jogo_id) if todos_jogos else None
        
        for usuario in usuarios:
            if not usuario.get('jogoDeInteresse'):
                continue
                
            for jogo in usuario['jogoDeInteresse']:
                if jogo['jogoId'] == jogo_id:
                    interessados.append({
                        'nome': usuario['nome'],
                        'email': usuario['email']
                    })
                    if not nome_jogo:
                        nome_jogo = jogo['nomeJogo']
                    break
        
        return {
            'interessados': interessados,
            'nome_jogo': nome_jogo or f"ID {jogo_id}"
        }

    @staticmethod
    def listar_jogos(jogos: List[Dict]) -> None:
        if not jogos:
            print("Nenhum jogo disponível encontrado.")
            return
            
        print("Jogos")
        print("Nome".ljust(20) + "ID")
        print("-" * 30)
        
        for jogo in sorted(jogos, key=lambda x: x['nome']):
            print(f"{jogo['nome'].ljust(20)}{jogo['id']}")
        
        print("\nDigite o ID do jogo que deseja consultar:")

    @staticmethod
    def obter_nome_jogo(jogos: List[Dict], jogo_id: int) -> str:
        if not jogos:
            return ""
        for jogo in jogos:
            if jogo['id'] == jogo_id:
                return jogo['nome']
        return ""