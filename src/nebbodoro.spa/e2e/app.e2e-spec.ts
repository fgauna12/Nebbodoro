import { Nebbodoro.SpaPage } from './app.po';

describe('nebbodoro.spa App', () => {
  let page: Nebbodoro.SpaPage;

  beforeEach(() => {
    page = new Nebbodoro.SpaPage();
  });

  it('should display welcome message', done => {
    page.navigateTo();
    page.getParagraphText()
      .then(msg => expect(msg).toEqual('Welcome to app!!'))
      .then(done, done.fail);
  });
});
