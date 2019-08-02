# Ludo API

API for a game of Ludo

## How to use

### Library

You need the following package to use this API.

`npm install @aspnet/signalr@next`

### Connecting

The API is live on `ludo.azurewebsites.net`

Connect on subpath `/game`

### Events

#### Lobbies

- invoke `lobby:create` with `lobbyName` to create a new lobby
- invoke `lobby:join` with `lobbyName` to join a created lobby
- invoke `lobby:leave` to leave your currently joined lobby
- invoke `lobby:ready` to inform your mates you're ready to roll
- invoke `lobby:get-players` with `lobbyName` to get the players in said lobby
- invoke `lobby:get-lobbies` to get the lobbies names
- invoke `lobby:exists` with `lobbyName` to check whether a lobby with that name already exists

When you're in a lobby you can get events for said lobby

- on `lobby:player-join` it returns the user details of the player that joined
- on `lobby:player-leave` it returns the user identifier for the player that left
- on `lobby:player-ready` it returns the player identifier of the player who is ready to start the game

#### Game

- invoke `game:start` as admin (those who created the lobby) to start the game when everyone is ready
- invoke `game:roll-die` to roll your dice if it's your turn
- invoke `game:advance` with `pieceIndex` to move the piece

When you're in a started game you can get events for that game

- on `game:started` the game has started
- on `game:die-roll` a player has rolled the dice, it returns the player identifier of the player who rolled the die and the amount it rolled
- on `game:advanced` a player has advanced his piece, it returns the player identifier of the player who advanced his piece and the index of said piece
- on `game:next-turn` the next player or turn starts, it returns the player identifier of the player whose turn it is and the turn type (roll or advance)
