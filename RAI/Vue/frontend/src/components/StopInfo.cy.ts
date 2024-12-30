import StopInfo from './StopInfo.vue'

describe('<StopInfo />', () => {
  it('renders', () => {
    // see: https://on.cypress.io/mounting-vue
    cy.mount(StopInfo, {
      props: {
        stopId: 1,
      }
    })
  })
})