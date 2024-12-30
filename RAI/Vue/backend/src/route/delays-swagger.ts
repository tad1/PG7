/**
 * @swagger
 * /api/delays/{id}:
 *   get:
 *     summary: Get delays for a specific stop
 *     description: Fetches real-time delay information for vehicles arriving at a specific stop.
 *     tags:
 *       - Delays
 *     parameters:
 *       - in: path
 *         name: id
 *         required: true
 *         schema:
 *           type: integer
 *         description: The stop ID for which delay information is requested.
 *     responses:
 *       200:
 *         description: Delay information for vehicles at the specified stop
 *         content:
 *           application/json:
 *             schema:
 *               type: object
 *               properties:
 *                 lastUpdate:
 *                   type: string
 *                   format: date-time
 *                   description: The timestamp of the last update.
 *                   example: "2024-12-17 15:31:51"
 *                 delay:
 *                   type: array
 *                   description: A list of delay information for vehicles.
 *                   items:
 *                     type: object
 *                     properties:
 *                       id:
 *                         type: string
 *                         description: A unique identifier for the delay entry.
 *                         example: "T12R171"
 *                       delayInSeconds:
 *                         type: integer
 *                         description: The delay in seconds. A negative value indicates the vehicle is early.
 *                         example: -130
 *                       estimatedTime:
 *                         type: string
 *                         description: The estimated time of arrival at the stop.
 *                         example: "15:43"
 *                       headsign:
 *                         type: string
 *                         description: The destination of the vehicle.
 *                         example: "Oliwa"
 *                       routeId:
 *                         type: integer
 *                         description: The ID of the route the vehicle is on.
 *                         example: 171
 *                       tripId:
 *                         type: integer
 *                         description: The ID of the trip the vehicle is currently on.
 *                         example: 12
 *                       status:
 *                         type: string
 *                         description: The status of the delay information (e.g., REALTIME, SCHEDULED).
 *                         example: "REALTIME"
 *                       theoreticalTime:
 *                         type: string
 *                         description: The scheduled arrival time.
 *                         example: "15:46"
 *                       timestamp:
 *                         type: string
 *                         description: The timestamp of the delay entry.
 *                         example: "15:31:51"
 *                       trip:
 *                         type: integer
 *                         description: An additional identifier for the trip.
 *                         example: 4648432
 *                       vehicleCode:
 *                         type: integer
 *                         description: The code of the vehicle.
 *                         example: 8507
 *                       vehicleId:
 *                         type: integer
 *                         description: The unique ID of the vehicle.
 *                         example: 145643
 *       500:
 *         description: Server error occurred while fetching delay information
 */