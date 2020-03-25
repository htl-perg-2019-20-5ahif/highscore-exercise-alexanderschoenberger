# API
`docker build -t highscoreapi HighScoreAPI/HighScoreAPI`


`docker run -p 5000:5000 -d highscoreapi`
# GAME
`docker build -t highscoregame ts-space-shooter-starter`


`docker run -p 8080:80 -d highscoregame`

# Docker-compose
`cd HighScoreAPI`


`docker-compose up`