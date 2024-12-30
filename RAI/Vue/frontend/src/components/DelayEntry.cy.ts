import DelayEntry from './DelayEntry.vue'
import '../index.css'


describe('<DelayEntry />', () => {
  it('renders', () => {
    // see: https://on.cypress.io/mounting-vue
    cy.mount(DelayEntry, {
      props: {
        data:{
          theoreticalTime: '15:00',
          headsign: 'Example Head Sign',
          routeShortName: "R",
          delayInSeconds: 12
        }
      }
    })
    cy.get('.theoretical-time').should('have.text', '15:00')
    cy.get('.headsign').should('have.text', 'Example Head Sign')
    cy.get('.route-short-name').should('have.text', 'R')
    cy.get('.delay').should('have.text', '0.2 min')
  })
  it('display green, when delayed', () => {
    cy.mount(DelayEntry, {
      props: {
        data:{
          theoreticalTime: '15:00',
          headsign: 'Example Head Sign',
          routeShortName: "R",
          delayInSeconds: 12
        }
      }
    })
    cy.get('.delay').should('have.class', 'text-green-500')
  })
  it('display red, when negative delay', () => {
    cy.mount(DelayEntry, {
      props: {
        data:{
          theoreticalTime: '15:00',
          headsign: 'Example Head Sign',
          routeShortName: "R",
          delayInSeconds: -12
        }
      }
    })
    cy.get('.delay').should('have.class', 'text-red-500')
  })
  it('display gray, when zero delay', () => {
    cy.mount(DelayEntry, {
      props: {
        data:{
          theoreticalTime: '15:00',
          headsign: 'Example Head Sign',
          routeShortName: "R",
          delayInSeconds: 0
        }
      }
    })
    cy.get('.delay').should('have.class', 'text-gray-700')
  })
})