FROM ruby:3.3
WORKDIR /app
EXPOSE 4000
EXPOSE 35729

RUN gem install jekyll bundler
COPY Gemfile Gemfile.lock ./
RUN bundle install 
COPY . . 

CMD [ "bundle", "exec", "jekyll", "serve", "--livereload", "--host", "0.0.0.0", "--force-polling" ]