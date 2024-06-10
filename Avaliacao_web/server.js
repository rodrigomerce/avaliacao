const fastify = require('fastify')()
const { Pool } = require('pg')
const cors = require('@fastify/cors')

const pool = new Pool({
  user: 'postgres',
  host: 'localhost',
  database: 'postgres',
  password: '76577614',
  port: 5432
})

fastify.register(cors)

fastify.get('/users', async (request, reply) => {
  try {
    const client = await pool.connect()
    const result = await client.query('SELECT * FROM users ORDER BY ID')
    const users = result.rows
    console.log(users)
    reply.send(users)
    client.release()
  } catch (err) {
    console.error('Error executing query', err)
    reply.status(500).send({ error: 'Internal Server Error' })
  }
})

// Rota para atualização de usuário
fastify.put('/users/:id', async (request, reply) => {
  const userId = request.params.id
  const { name, email } = request.body // Supondo que o corpo da requisição contenha os novos valores do nome e do email

  try {
    const client = await pool.connect()
    const result = await client.query(
      'UPDATE users SET name_firstname = $1, email = $2 WHERE id = $3',
      [name, email, userId]
    )
    console.log(`User with ID ${userId} updated`)
    reply.send({ message: `User with ID ${userId} updated` })
    client.release()
  } catch (err) {
    console.error('Error updating user', err)
    reply.status(500).send({ error: 'Internal Server Error' })
  }
})

fastify.put('/updateuser/:id', async (request, reply) => {
  const userId = request.params.id
  const {
    Name_firstName,
    Name_LastName,
    email,
    gender,
    Location_Street_Number,
    Location_Street_Name,
    Location_City,
    Location_State,
    Location_Country,
    Location_Pastcode,
    Login_Username,
    Login_Password,
    phone,
    cell
  } = request.body // Supondo que o corpo da requisição contenha os novos valores do nome e do email

  console.log(request.body)
  try {
    const client = await pool.connect()
    const result = await client.query(
      `UPDATE users 
       SET 
         Name_firstName = $1, 
         name_lastname = $2, 
         email = $3, 
         gender = $4,
         location_street_number = $5, 
         location_street_name = $6, 
         location_city = $7, 
         location_state = $8, 
         location_country = $9, 
         location_pastcode = $10, 
         login_username = $11, 
         login_password = $12, 
         phone = $13, 
         cell = $14
       WHERE id = $15`,
      [
        Name_firstName,
        Name_LastName,
        email,
        gender,
        Location_Street_Number,
        Location_Street_Name,
        Location_City,
        Location_State,
        Location_Country,
        Location_Pastcode,
        Login_Username,
        Login_Password,
        phone,
        cell,
        userId
      ]
    )
    console.log(`User with ID ${userId} updated`)
    reply.send({ message: `User with ID ${userId} updated` })
    client.release()
  } catch (err) {
    console.error('Error updating user', err)
    reply.status(500).send({ error: 'Internal Server Error' })
  }
})

fastify
  .listen({
    port: 3000
  })
  .then(() => {
    console.log('HTTP Server running!')
  })
