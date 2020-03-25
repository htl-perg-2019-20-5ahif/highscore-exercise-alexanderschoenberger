import { Scene, Types, CANVAS, Game, Physics, Input } from 'phaser';
import { Spaceship, Direction } from './spaceship';
import { Bullet } from './bullet';
import { Meteor } from './meteor';
import { request } from 'http';

/**
 * Space shooter scene
 * https://wanago.io/2019/03/18/node-js-typescript-6-sending-http-requests-understanding-multipart-form-data/
 * Learn more about Phaser scenes at 
 * https://photonstorm.github.io/phaser3-docs/Phaser.Scenes.Systems.html.
 */
class ShooterScene extends Scene {
    private spaceShip: Spaceship;
    private meteors: Physics.Arcade.Group;
    private bullets: Physics.Arcade.Group;

    private bulletTime = 0;
    private meteorTime = 0;

    private cursors: Types.Input.Keyboard.CursorKeys;
    private spaceKey: Input.Keyboard.Key;
    private isGameOver = false;
    private count = 0;
    private host = 'highscoreapi';
    private path = '/api/Highscores';

    preload() {
        // Preload images so that we can use them in our game
        this.load.image('space', 'images/deep-space.jpg');
        this.load.image('bullet', 'images/scratch-laser.png');
        this.load.image('ship', 'images/scratch-spaceship.png');
        this.load.image('meteor', 'images/scratch-meteor.png');
    }

    create() {
        this.getHighScores();
        if (this.isGameOver) {
            return;
        }

        //  Add a background
        this.add.tileSprite(0, 0, this.game.canvas.width, this.game.canvas.height, 'space').setOrigin(0, 0);

        // Create bullets and meteors
        this.bullets = this.physics.add.group({
            classType: Bullet,
            maxSize: 10,
            runChildUpdate: true
        });
        this.meteors = this.physics.add.group({
            classType: Meteor,
            maxSize: 20,
            runChildUpdate: true
        });

        // Add the sprite for our space ship.
        this.spaceShip = new Spaceship(this);
        this.physics.add.existing(this.children.add(this.spaceShip));

        // Position the spaceship horizontally in the middle of the screen
        // and vertically at the bottom of the screen.
        this.spaceShip.setPosition(this.game.canvas.width / 2, this.game.canvas.height * 0.9);

        // Setup game input handling
        this.cursors = this.input.keyboard.createCursorKeys();
        this.input.keyboard.addCapture([' ']);
        this.spaceKey = this.input.keyboard.addKey(Input.Keyboard.KeyCodes.SPACE);
        const score = this.add.text(30, 30, 'Score: ' + this.count,
            { font: "15px Arial", fill: "#ff0044", align: "center" });

        score.setOrigin(0.5, 0.5);
        this.physics.add.collider(this.bullets, this.meteors, (bullet: Bullet, meteor: Meteor) => {

            meteor.kill();
            bullet.kill();
        }, (bullet: Bullet, meteor: Meteor) => {
            if (meteor.active) {
                this.count++;
                score.setText('Score: ' + this.count)

            }
            meteor.kill();
            bullet.kill();


        }, this);

        this.physics.add.collider(this.spaceShip, this.meteors, this.gameOver, null, this);
    }

    update(_, delta: number) {
        // Move ship if cursor keys are pressed
        if (this.cursors.left.isDown) {
            this.spaceShip.move(delta, Direction.Left);
        }
        else if (this.cursors.right.isDown) {
            this.spaceShip.move(delta, Direction.Right);
        }

        if (this.spaceKey.isDown) {
            this.fireBullet();
        }

        this.handleMeteors();
    }

    fireBullet() {
        if (this.time.now > this.bulletTime) {
            // Find the first unused (=unfired) bullet
            const bullet = this.bullets.get() as Bullet;
            if (bullet) {
                bullet.fire(this.spaceShip.x, this.spaceShip.y);
                this.bulletTime = this.time.now + 100;
            }
        }
    }

    handleMeteors() {
        // Check if it is time to launch a new meteor.
        if (this.time.now > this.meteorTime) {
            // Find first meteor that is currently not used
            const meteor = this.meteors.get() as Meteor;
            if (meteor) {
                meteor.fall();
                this.meteorTime = this.time.now + 500 + 1000 * Math.random();
            }
        }
    }
    sendPlayerScore(ref) {
        const user = (document.getElementById("txt_name") as HTMLInputElement).value;
        const req = request(
            {
                host: ref.host,
                path: ref.path,
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                }
            },

            response => {
                response.on('data', d => {
                    console.log("DATA", d);
                });
                req.on('error', error => {
                    console.error('error', error)
                });
            }

        );
        // add token
        console.log('User', user, ref.count);
        req.write(JSON.stringify({
            'highscore': {
                'user': user,
                'score': ref.count
            },
            'captcha': (document.getElementById('g-recaptcha-response') as HTMLInputElement).value
        }));

        req.end();
    }

    gameOver() {
        this.isGameOver = true;

        this.bullets.getChildren().forEach((b: Bullet) => b.kill());
        this.meteors.getChildren().forEach((m: Meteor) => m.kill());

        // Display "game over" text
        const text = this.add.text(this.game.canvas.width / 2, this.game.canvas.height / 2, "Game Over :-(",
            { font: "65px Arial", fill: "#ff0044", align: "center" });
        text.setOrigin(0.5, 0.5);
        if (this.spaceShip.active) {
            this.spaceShip.kill();

            document.getElementById("send").addEventListener("click", (e: Event) => this.sendPlayerScore(this));

            this.getHighScores();
        }
    }

    private getHighScores() {
        const getReq = request({
            host: this.host,
            path: this.path,
            method: 'GET',
        }, response => {
            console.log("GET", response.statusCode); // 200
            const chunks = [];
            response.on('data', (chunk) => {
                chunks.push(chunk);
            });
            response.on('end', () => {
                const data = Buffer.concat(chunks).toString();
                const result = {
                    data: JSON.parse(data)
                };
                let highscore = "Result: " + "(length=" + result.data.length + ")<br>";
                highscore += "<table><tr><th>highScoreId</th> <th>score</th> <th>user</th></tr>";
                for (const element of result.data) {
                    highscore += "<tr><td> " + element.highScoreId + "</td> <td>"
                        + element.score + "</td><td> " + element.user + "</td></tr>";
                }
                highscore += "</table>";
                document.getElementById("highscores").innerHTML = highscore;
                console.log(result.data);
            });
        });
        getReq.end();
    }
}

const config = {
    type: CANVAS,
    width: 512,
    height: 512,
    scene: [ShooterScene],
    physics: { default: 'arcade' },
    audio: { noAudio: true }
};

new Game(config);
