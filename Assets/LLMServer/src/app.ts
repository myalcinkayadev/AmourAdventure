import { PromptTemplate } from "@langchain/core/prompts";
import { RunnableMap } from "@langchain/core/runnables";
import { Ollama } from "@langchain/ollama";

async function main() {
  const model = new Ollama({
    model: "llama3.2:latest",
    temperature: 0,
    maxRetries: 2,
  });

  const jokeChain = PromptTemplate.fromTemplate(
    "Tell me a joke about {topic}"
  ).pipe(model);
  const poemChain = PromptTemplate.fromTemplate(
    "write a 2-line poem about {topic}"
  ).pipe(model);

  const mapChain = RunnableMap.from({
    joke: jokeChain,
    poem: poemChain,
  });

  const result = await mapChain.invoke({ topic: "bear" });
  console.log(JSON.stringify(result, null, 2))
}

main()
