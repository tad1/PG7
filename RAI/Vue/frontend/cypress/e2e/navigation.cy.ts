import { v4 } from "uuid";


Cypress.Commands.add('login', (username: string, password: string) => {
  cy.visit('/login')
  cy.get('input[id="username"]').type(username);
  cy.get('input[id="password"]').type(password);
  cy.get('input[type="submit"]').click()
});

describe('check if navigation bar works', () => {
  const username = v4();
  before(() => {
    cy.visit('/register')
    cy.get('input[id="username"]').type(username)
    cy.get('input[id="password"]').type('password')
    cy.get('input[id="confirm-password"]').type('password')
    cy.get('input[type="submit"]').click()
  })

  it('home, should navigate to main page', () => {
      cy.visit('/')
      cy.get('#home-nav').click()
      cy.location('pathname').should('eq', '/')
    })

  it('saved, should navigate to saved page', () => {
      cy.login(username, 'password');
      cy.get('#saved-nav').click()
      cy.location('pathname').should('eq', '/saved')
    })

  it('logout, should navigate to login page', () => {
      cy.visit('/')
      cy.get('#login-nav').click()
      cy.location('pathname').should('eq', '/login')
  })

  it('register, should navigate to register page', () => {
      cy.visit('/')
      cy.get('#register-nav').click()
      cy.location('pathname').should('eq', '/register')
    })
})