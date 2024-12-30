describe('check if app displays stuff', () => {
    it('should show 10 bus stops', () => {
      cy.visit('/')
      cy.get('.stop-info').should('have.length', 10)
    })
  })