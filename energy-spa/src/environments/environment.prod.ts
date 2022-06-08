import { Configuration, version } from './configuration';

const getCurrentOrigin = () => {
  return window ? window.location.origin : '.';
};

export const environment: Configuration = {
  production: true,
  debug: false,
  version,
  serverUri: 'http://localhost:5000', //getCurrentOrigin(),
};
