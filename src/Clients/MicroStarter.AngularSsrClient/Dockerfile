FROM node:9-alpine

# Create app directory
WORKDIR /usr/src/app

# Install app dependencies
# A wildcard is used to ensure both package.json AND package-lock.json are copied
# where available (npm@5+)
COPY package*.json /tmp/

RUN cd /tmp && npm install
RUN mkdir -p /usr/src/app && cp -a /tmp/node_modules /usr/src/app


# Bundle app source
WORKDIR /usr/src/app
COPY . /usr/src/app

# Build App
ENV PORT=4200  
RUN npm run build:ssr
EXPOSE 4200
CMD npm run serve:ssr