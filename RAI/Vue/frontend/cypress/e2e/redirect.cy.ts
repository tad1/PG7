describe('check if redirect works', () => {
  it('should redirect unauthenticated user to signin page', () => {
    cy.visit('/saved')
    cy.location('pathname').should('eq', '/login')
  })
})