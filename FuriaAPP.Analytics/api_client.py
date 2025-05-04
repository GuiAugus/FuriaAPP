import requests
from typing import List, Dict, Optional
from config import APIConfig

class APIClient:
    def __init__(self, config: APIConfig = APIConfig):
        self.config = config
    
    def get_jogos(self) -> Optional[List[Dict]]:
        return self._make_request('jogos')
    
    def get_usuarios(self) -> Optional[List[Dict]]:
        return self._make_request('usuarios')
    
    def _make_request(self, endpoint_key: str) -> Optional[List[Dict]]:
        url = self.config.get_full_url(endpoint_key)
        try:
            response = requests.get(url)
            if response.status_code == 200:
                return response.json()
            print(f"Erro ao obter {endpoint_key}: {response.status_code}")
            return None
        except Exception as e:
            print(f"Erro na requisição de {endpoint_key}: {str(e)}")
            return None