# Space Battle 🚀👾

Space Battle é um jogo de batalha espacial desenvolvido em C# utilizando o framework WPF. O jogador controla uma nave que deve evitar o impacto com naves inimigas e eliminá-las disparando tiros. O objetivo é derrotar o maior número de inimigos possível antes que sua nave seja destruída.

## Funcionalidades

- Movimentação do jogador usando as teclas direcionais (esquerda/direita).
- Disparo de tiros usando a barra de espaço.
- Sistema de pontuação baseado no número de inimigos destruídos.
- Aumento da dificuldade conforme o jogador progride (velocidade dos inimigos aumenta).
- Detecção de colisão entre a nave do jogador, inimigos e projéteis.
- Exibição do dano causado à nave do jogador.
- Game over ao atingir o limite de dano.

## Tecnologias Utilizadas

- **C#** - Linguagem de programação.
- **WPF (Windows Presentation Foundation)** - Framework para a criação da interface gráfica e elementos de jogo.
- **XAML** - Usado para definir a interface visual e os componentes gráficos.
- **DispatcherTimer** - Utilizado para controlar o ciclo principal do jogo (game loop).
- **Canvas** - Componente principal para renderizar os elementos do jogo (nave, inimigos, tiros).

## Como Executar

1. Clone este repositório para sua máquina local usando o comando:

    ```bash
    git clone https://github.com/seu-usuario/space-battle.git
    ```

2. Abra o projeto no Visual Studio.

3. Restaure os pacotes NuGet, se necessário.

4. Compile e execute o projeto.

## Como Jogar

- **Movimentar a nave**: Use as setas esquerda (⬅️) e direita (➡️) para mover a nave.
- **Disparar**: Pressione a barra de espaço para atirar.
- **Objetivo**: Destrua o máximo de naves inimigas que puder. Se as naves inimigas colidirem com sua nave ou atingirem a parte inferior da tela, você sofrerá dano.
- **Game Over**: O jogo termina quando o dano da sua nave atinge 100%.

## Melhorias Futuras

- Adicionar novos tipos de inimigos e padrões de movimentação.
- Implementar power-ups que melhorem as habilidades da nave do jogador.
- Sistema de níveis com diferentes desafios e cenários.
- Adicionar trilha sonora e efeitos sonoros.

### Autor

Wesley Goncalves Alves - Desenvolvedor C#/.NET

- [LinkedIn](https://www.linkedin.com/in/wesley-gon%C3%A7alves-alves-3b95472ab/)
- [GitHub](https://github.com/WesleyGAlves)
