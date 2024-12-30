
/**
 * @swagger
 * tags:
 *   - name: Stops
 *     description: Operations related to stops
 *   - name: Saved Stops
 *     description: Manage user's saved stops
 */

/**
 * @swagger
 * /:
 *   get:
 *     summary: Health check for the app
 *     description: Returns a simple "Hello World!" message.
 *     tags:
 *       - Health Check
 *     responses:
 *       200:
 *         description: A welcome message
 *         content:
 *           application/json:
 *             schema:
 *               type: object
 *               properties:
 *                 app:
 *                   type: string
 *                   example: Hello World!
 */

/**
 * @swagger
 * /api/stops:
 *   get:
 *     summary: Get all current stops
 *     description: Returns a list of all stops available for the current date.
 *     tags:
 *       - Stops
 *     parameters:
 *       - in: query
 *         name: page
 *         required: false
 *         description: The page number to retrieve.
 *         schema:
 *           type: integer
 *           default: 1
 *       - in: query
 *         name: limit
 *         required: false
 *         description: The number of stops per page.
 *         schema:
 *           type: integer
 *           default: 10
 *     responses:
 *       200:
 *         description: A list of current stops
 *         content:
 *           application/json:
 *             schema:
 *               type: array
 *               items:
 *                 type: object
 *                 properties:
 *                   stopId:
 *                     type: integer
 *                   stopCode:
 *                     type: string
 *                   stopName:
 *                     type: string
 *                   stopShortName:
 *                     type: string
 *                   stopDesc:
 *                     type: string
 *                   subName:
 *                     type: string
 *                   zoneId:
 *                     type: integer
 *                   zoneName:
 *                     type: string
 *                   stopLat:
 *                     type: number
 *                     format: float
 *                   stopLon:
 *                     type: number
 *                     format: float
 *                   type:
 *                     type: string
 */

/**
 * @swagger
 * /api/stops/{id}:
 *   get:
 *     summary: Get details of a specific stop
 *     description: Returns details of a stop identified by its ID.
 *     tags:
 *       - Stops
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         schema:
 *           type: integer
 *         description: ID of the stop to retrieve
 *     responses:
 *       200:
 *         description: Details of the requested stop
 *         content:
 *           application/json:
 *             schema:
 *               type: object
 *               properties:
 *                 stopId:
 *                   type: integer
 *                 stopCode:
 *                   type: string
 *                 stopName:
 *                   type: string
 *                 stopShortName:
 *                   type: string
 *                 stopDesc:
 *                   type: string
 *                 subName:
 *                   type: string
 *                 zoneId:
 *                   type: integer
 *                 zoneName:
 *                   type: string
 *                 stopLat:
 *                   type: number
 *                   format: float
 *                 stopLon:
 *                   type: number
 *                   format: float
 *                 type:
 *                   type: string
 *       404:
 *         description: Stop not found
 */

/**
 * @swagger
 * /api/saved:
 *   get:
 *     summary: Get user's saved stops
 *     description: Returns a list of stops saved by the authenticated user.
 *     tags:
 *       - Saved Stops
 *     security:
 *       - bearerAuth: []
 *     responses:
 *       200:
 *         description: A list of saved stops
 *         content:
 *           application/json:
 *             schema:
 *               type: array
 *               items:
 *                 type: object
 *                 properties:
 *                   stopId:
 *                     type: integer
 *                   username:
 *                     type: string
 */

/**
 * @swagger
 * /api/saved/add/{id}:
 *   post:
 *     summary: Add a stop to the user's saved stops
 *     description: Adds a stop to the authenticated user's saved stops list.
 *     tags:
 *       - Saved Stops
 *     security:
 *       - bearerAuth: []
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         schema:
 *           type: integer
 *         description: ID of the stop to save
 *     responses:
 *       200:
 *         description: Stop successfully saved
 *       400:
 *         description: Invalid request or failure to save the stop
 */

/**
 * @swagger
 * /api/saved/remove:
 *   delete:
 *     summary: Remove a stop from the user's saved stops
 *     description: Removes a stop from the authenticated user's saved stops list.
 *     tags:
 *       - Saved Stops
 *     security:
 *       - bearerAuth: []
 *     requestBody:
 *       required: true
 *       content:
 *         application/json:
 *           schema:
 *             type: object
 *             properties:
 *               stopId:
 *                 type: integer
 *     responses:
 *       200:
 *         description: Stop successfully removed
 *       400:
 *         description: Invalid request or failure to remove the stop
 */