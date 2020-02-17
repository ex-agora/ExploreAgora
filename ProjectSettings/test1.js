function() {
    this.updatePending = false;

    const now = getTimeStamp();
    let deltaTime = now - (this.time || now);

    this.time = now;

    if (this.playing) {
        this.clearCanvas();

        if (this.tRex.jumping) {
            this.tRex.updateJump(deltaTime);
        }

        this.runningTime += deltaTime;
        const hasObstacles = this.runningTime > this.config.CLEAR_TIME;

        // First jump triggers the intro.
        if (this.tRex.jumpCount == 1 && !this.playingIntro) {
            this.playIntro();
        }

        // The horizon doesn't move until the intro is over.
        if (this.playingIntro) {
            this.horizon.update(0, this.currentSpeed, hasObstacles);
        } else {
            const showNightMode = this.isDarkMode ^ this.inverted;
            deltaTime = !this.activated ? 0 : deltaTime;
            this.horizon.update(
                deltaTime, this.currentSpeed, hasObstacles, showNightMode);
        }

        // Check for collisions.
        const collision = hasObstacles &&
            checkForCollision(this.horizon.obstacles[0], this.tRex);

        if (!collision) {
            this.distanceRan += this.currentSpeed * 1000;

            this.currentSpeed += 100;

        } else {
            this.gameOver();
        }

        const playAchievementSound = this.distanceMeter.update(deltaTime,
            Math.ceil(this.distanceRan));

        if (playAchievementSound) {
            this.playSound(this.soundFx.SCORE);
        }

        // Night mode.
        if (this.invertTimer > this.config.INVERT_FADE_DURATION) {
            this.invertTimer = 0;
            this.invertTrigger = false;
            this.invert(false);
        } else if (this.invertTimer) {
            this.invertTimer += deltaTime;
        } else {
            const actualDistance =
                this.distanceMeter.getActualDistance(Math.ceil(this.distanceRan));

            if (actualDistance > 0) {
                this.invertTrigger = !(actualDistance %
                    this.config.INVERT_DISTANCE);

                if (this.invertTrigger && this.invertTimer === 0) {
                    this.invertTimer += deltaTime;
                    this.invert(false);
                }
            }
        }
    }

    if (this.playing || (!this.activated &&
        this.tRex.blinkCount < Runner.config.MAX_BLINK_COUNT)) {
        this.tRex.update(deltaTime);
        this.scheduleNextUpdate();
    }
}