io.stdout:setvbuf('no')

function love.load()
	anim8 = require 'libraries/anim8'
	love.graphics.setDefaultFilter("nearest", "nearest")

	player = {}
	player.x = 400
	player.y = 200
	player.speed = 5

	player.spriteSheet = love.graphics.newImage('sprites/player-sheet.png')
	player.grid = anim8.newGrid(12, 18, player.spriteSheet:getWidth(), player.spriteSheet:getHeight())

	player.animations = {}
	player.animations.down = anim8.newAnimation( player.grid('1-4', 1), 0.2)
	player.animations.left = anim8.newAnimation( player.grid('1-4', 2), 0.2)
	player.animations.right = anim8.newAnimation( player.grid('1-4', 3), 0.2)
	player.animations.up = anim8.newAnimation( player.grid('1-4', 4), 0.2)
	player.anim = player.animations.left

	--player.sprite = love.graphics.newImage('sprites/icon.png')
	background = love.graphics.newImage("sprites/4kwhite.png")
end

function love.update(dt)
	local isMoving = false
	if love.keyboard.isDown("d") then
		player.x = player.x + player.speed
		player.anim = player.animations.right
		isMoving = true
	end
	if love.keyboard.isDown("a") then
		player.x = player.x - player.speed
		player.anim = player.animations.left
		isMoving = true
	end
	if love.keyboard.isDown("w") then
		player.y = player.y - player.speed
		player.anim = player.animations.up
		isMoving = true

	end
	if love.keyboard.isDown("s") then
		player.y = player.y + player.speed
		player.anim = player.animations.down
		isMoving = true
	end
	if not isMoving then
		player.anim:gotoFrame(2)
	end
	player.anim:update(dt)
end

function love.draw()
	love.graphics.draw(background, 0, 0)-- draw the background first so that everything else goes on top of this
	--love.graphics.circle("fill", player.x, player.y, 100)
	--love.graphics.draw(player.sprite, player.x, player.y)
	player.anim:draw(player.spriteSheet, player.x, player.y, nil, 10)
end