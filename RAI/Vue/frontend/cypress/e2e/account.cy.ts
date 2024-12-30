import { v4 } from "uuid";

Cypress.Commands.add('login', (username: string, password: string) => {
    cy.visit('/login')
    cy.get('input[id="username"]').type(username);
    cy.get('input[id="password"]').type(password);
    cy.get('input[type="submit"]').click()
});

describe('check if account works', () => {
    const username = v4();
    it('should register new user', () => {
        cy.visit('/register')
        cy.location('pathname').should('eq', '/register')
        cy.focused().should('have.attr', 'id', 'username')
        cy.get('input[id="username"]').type(username)
        cy.get('input[id="password"]').type('password')
        cy.get('input[id="confirm-password"]').type('password')
        cy.get('input[type="submit"]').click()
        cy.location('pathname').should('eq', '/login')
    })

    it('should login user', () => {
        cy.visit('/login')
        cy.location('pathname').should('eq', '/login')
        cy.focused().should('have.attr', 'id', 'username')
        cy.get('input[id="username"]').type(username)
        cy.get('input[id="password"]').type('password')
        cy.get('input[type="submit"]').click()
        cy.location('pathname').should('eq', '/saved')
    })

    it('should display username', () => {
        cy.visit('/saved')
        cy.login(username, 'password');
        cy.get('#saved-nav').should('contain', username)
    })

    it('should display logout', () => {
        cy.visit('/saved')
        cy.login(username, 'password');
        cy.get('#logout-btn').should('contain', 'Logout')
    })
  });