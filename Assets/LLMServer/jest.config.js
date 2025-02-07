module.exports = {
  moduleFileExtensions: ['js', 'ts', 'json'],
  transform: {
    '^.+\\.ts$': '@swc/jest',
  },
  testEnvironment: 'node',
  setupFiles: ['<rootDir>/test/setup.ts'],
  roots: ['src', 'test'],
};
