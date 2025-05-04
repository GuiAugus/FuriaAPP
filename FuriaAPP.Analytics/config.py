from typing import Dict

class APIConfig:
    BASE_URL = "http://localhost:5101/"
    ENDPOINTS = {
        'jogos': 'api/competicoes/jogos',
        'usuarios': 'api/usuario'
    }

    @classmethod
    def get_full_url(cls, endpoint_key: str) -> str:
        return f"{cls.BASE_URL}{cls.ENDPOINTS[endpoint_key]}"